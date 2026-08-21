using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Net;
using System.Net.Sockets;
using Wyrm.ModBusClient.GivEnergy.DependencyInjection;
using Wyrm.ModBusClient.GivEnergy.Extensions;
using Wyrm.ModBusClient.GivEnergy.Responses;
using Wyrm.ModBusClient.GivEnergy.Responses.Constants;
using Wyrm.ModBusClient.GivEnergy.Services;

namespace Wyrm.ModBusClient.GivEnergy.IntegrationTests;

public class GivEnergyClientTests : IDisposable
{
    #region Setup

    private readonly IGivEnergyClient _givEnergyClient;
    private readonly ICheckSumService _checkSumService;
    private readonly System.Net.Sockets.Socket _inverter = new(TestEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

    private static readonly EndPoint TestEndPoint = new IPEndPoint(new IPAddress([127, 0, 0, 1]), 8000);

    public GivEnergyClientTests()
    {
        var services = new ServiceCollection().AddGivEnergyClient();
        var provider = services.BuildServiceProvider();
        _givEnergyClient = provider.GetRequiredService<IGivEnergyClient>();
        _checkSumService = new CheckSumService();
        _inverter.Bind(TestEndPoint);
        _inverter.Listen();
    }

    public void Dispose()
    {
        _inverter.Close();
        GC.SuppressFinalize(this);
    }

    #endregion

    private const int RegisterBytes = 120;
    private const string WifiHost = "WH12345678";
    private const string SerialNo = "SN12345678";

    public static readonly TheoryData<ResponseDataType, byte, IEnumerable<byte>, IResponseData> RequestInverterTests = new()
    {
        { ResponseDataType.InverterProperties1, 0, Enumerable.Range(1, RegisterBytes).Select(x => (byte)x), new InverterProperties1() },
        { ResponseDataType.InverterData1, 0, Enumerable.Range(1, RegisterBytes).Select(x => (byte)x), new InverterData1() },
        // TODO
    };

    [Theory, MemberData(nameof(RequestInverterTests))]
    public async Task RequestInverterDataAsync_Should_GetInverterData(ResponseDataType responseDataType, byte deviceIndex, IEnumerable<byte> registerData, IResponseData expected)
    {
        var deviceAddress = responseDataType.DeviceAddress(deviceIndex);
        var functionNo = (byte)(responseDataType.InputRegisters() ? 4 : 3);
        var registerAddress = responseDataType.StartAddress();

        var inverterSocket = await ConnectAndAcceptAsync(TestContext.Current.CancellationToken);
        await _givEnergyClient.RequestInverterDataAsync(responseDataType, 0, TestContext.Current.CancellationToken);
        await ReceiveCommandAndCheckAsync(inverterSocket, deviceAddress, functionNo, registerAddress, TestContext.Current.CancellationToken);

        await SendResponseAsync(inverterSocket, deviceAddress, functionNo, registerAddress, registerData, TestContext.Current.CancellationToken);
        var response = await _givEnergyClient.WaitForResponseAsync(TestContext.Current.CancellationToken);

        _givEnergyClient.Close();

        response.SerialNumber.ShouldBe(SerialNo);
        response.WifiAdapter.ShouldBe(WifiHost);
        response.DeviceNumber.ShouldBe((byte)(deviceIndex + 1));
        response.ResponseDataType.ShouldBe(responseDataType);
        // TODO: Test response.ResponseData against expected
    }

    private async Task<System.Net.Sockets.Socket> ConnectAndAcceptAsync(CancellationToken cancellationToken)
    {
        var accept = _inverter.AcceptAsync(cancellationToken);
        await _givEnergyClient.ConnectAsync(TestEndPoint, cancellationToken);
        return await accept;
    }

    private async Task ReceiveCommandAndCheckAsync(System.Net.Sockets.Socket inverterSocket, byte deviceAddress, byte functionNo, ushort registerAddress, CancellationToken cancellationToken)
    {
        var buffer = new byte[1024];
        await inverterSocket.ReceiveAsync(buffer, cancellationToken);
        var expectedRequestData = new byte[] { deviceAddress, functionNo, 0, (byte)registerAddress, 0, 60 };
        var checkSumData = _checkSumService.CheckSum(expectedRequestData);
        byte[] expectedRequest = [89, 89, 0, 1, 0, 28, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, .. expectedRequestData, .. checkSumData];
        byte[] bufferCheck = [.. expectedRequest, .. Enumerable.Range(1, 1024 - expectedRequest.Length).Select(_ => (byte)0)];
        buffer.ShouldBeEquivalentTo(bufferCheck);
    }

    private async Task SendResponseAsync(System.Net.Sockets.Socket inverterSocket, byte deviceAddress, byte functionNo, ushort registerAddress, IEnumerable<byte> registerData, CancellationToken cancellationToken)
    {
        byte[] pduData = [deviceAddress, functionNo, .. SerialNo.Select(x => (byte)x), 0, (byte)registerAddress, 0, 60, .. registerData];
        var checkSumData = _checkSumService.CheckSum(pduData);
        byte[] responseData = [89, 89, 0, 1, 0, (byte)(pduData.Length + 22), 1, 2, .. WifiHost.Select(x => (byte)x), 0, 0, 0, 0, 0, 0, 0, (byte)(pduData.Length + 2), .. pduData, .. checkSumData];
        await inverterSocket.SendAsync(responseData, TestContext.Current.CancellationToken);
    }
}
