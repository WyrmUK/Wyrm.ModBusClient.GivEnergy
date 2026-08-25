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

    private static readonly byte[] InverterProperties1Data = [.. Enumerable.Range(1, RegisterBytes).Select(x => (byte)x)];
    private static readonly InverterProperties1 InverterProperties1Response = new();
    private static readonly byte[] InverterProperties2Data = [.. Enumerable.Range(1, RegisterBytes).Select(x => (byte)x)];
    private static readonly InverterProperties2 InverterProperties2Response = new();
    private static readonly byte[] InverterProperties3Data = [.. Enumerable.Range(1, RegisterBytes).Select(x => (byte)x)];
    private static readonly InverterProperties3 InverterProperties3Response = new();
    private static readonly byte[] InverterProperties4Data = [.. Enumerable.Range(1, RegisterBytes).Select(x => (byte)x)];
    private static readonly InverterProperties4 InverterProperties4Response = new();
    private static readonly byte[] InverterProperties5Data = [.. Enumerable.Range(1, RegisterBytes).Select(x => (byte)x)];
    private static readonly InverterProperties5 InverterProperties5Response = new();
    private static readonly byte[] InverterProperties6Data = [.. Enumerable.Range(1, RegisterBytes).Select(x => (byte)x)];
    private static readonly InverterProperties6 InverterProperties6Response = new();
    private static readonly byte[] InverterProperties9Data = [.. Enumerable.Range(1, RegisterBytes).Select(x => (byte)x)];
    private static readonly InverterProperties9 InverterProperties9Response = new();
    private static readonly byte[] InverterProperties10Data = [.. Enumerable.Range(1, RegisterBytes).Select(x => (byte)x)];
    private static readonly InverterProperties10 InverterProperties10Response = new();
    private static readonly byte[] InverterData1Data = [.. Enumerable.Range(1, RegisterBytes).Select(x => (byte)x)];
    private static readonly InverterData1 InverterData1Response = new();
    private static readonly byte[] InverterData5Data = [.. Enumerable.Range(1, RegisterBytes).Select(x => (byte)x)];
    private static readonly InverterData5 InverterData5Response = new();
    private static readonly byte[] LowVoltageBCUData2Data = [.. Enumerable.Range(1, RegisterBytes).Select(x => (byte)x)];
    private static readonly LowVoltageBCUData2 LowVoltageBCUData2Response = new();
    private static readonly byte[] BatteryData20Data = [.. Enumerable.Range(1, RegisterBytes).Select(x => (byte)x)];
    private static readonly BatteryData2 BatteryData20Response = new();
    private static readonly byte[] BatteryData21Data = [.. Enumerable.Range(1, RegisterBytes).Select(x => (byte)x)];
    private static readonly BatteryData2 BatteryData21Response = new();
    private static readonly byte[] MeterData20Data = [.. Enumerable.Range(1, RegisterBytes).Select(x => (byte)x)];
    private static readonly MeterData2 MeterData20Response = new();
    private static readonly byte[] MeterData21Data = [.. Enumerable.Range(1, RegisterBytes).Select(x => (byte)x)];
    private static readonly MeterData2 MeterData21Response = new();

    public static readonly TheoryData<ResponseDataType, byte, byte[], IResponseData> RequestInverterTests = new()
    {
        { ResponseDataType.InverterProperties1, 0, InverterProperties1Data, InverterProperties1Response },
        { ResponseDataType.InverterProperties2, 0, InverterProperties2Data, InverterProperties2Response },
        { ResponseDataType.InverterProperties3, 0, InverterProperties3Data, InverterProperties3Response },
        { ResponseDataType.InverterProperties4, 0, InverterProperties4Data, InverterProperties4Response },
        { ResponseDataType.InverterProperties5, 0, InverterProperties5Data, InverterProperties5Response },
        { ResponseDataType.InverterProperties6, 0, InverterProperties6Data, InverterProperties6Response },
        { ResponseDataType.InverterProperties9, 0, InverterProperties9Data, InverterProperties9Response },
        { ResponseDataType.InverterProperties10, 0, InverterProperties10Data, InverterProperties10Response },
        { ResponseDataType.InverterData1, 0, InverterData1Data, InverterData1Response },
        { ResponseDataType.InverterData5, 0, InverterData5Data, InverterData5Response },
        { ResponseDataType.LowVoltageBCUData2, 0, LowVoltageBCUData2Data, LowVoltageBCUData2Response },
        { ResponseDataType.BatteryData2, 0, BatteryData20Data, BatteryData20Response },
        { ResponseDataType.BatteryData2, 1, BatteryData21Data, BatteryData21Response },
        { ResponseDataType.MeterData2, 0, MeterData20Data, MeterData20Response },
        { ResponseDataType.MeterData2, 1, MeterData21Data, MeterData21Response }
    };

    [Theory, MemberData(nameof(RequestInverterTests))]
    public async Task RequestInverterDataAsync_Should_GetInverterData(ResponseDataType responseDataType, byte deviceIndex, IEnumerable<byte> registerData, IResponseData expected)
    {
        var deviceAddress = responseDataType.DeviceAddress(deviceIndex);
        var functionNo = (byte)(responseDataType.InputRegisters() ? 4 : 3);
        var registerAddress = responseDataType.StartAddress();

        var inverterSocket = await ConnectAndAcceptAsync(TestContext.Current.CancellationToken);
        await _givEnergyClient.RequestInverterDataAsync(responseDataType, deviceIndex, TestContext.Current.CancellationToken);
        await ReceiveCommandAndCheckAsync(inverterSocket, deviceAddress, functionNo, registerAddress, TestContext.Current.CancellationToken);

        await SendResponseAsync(inverterSocket, deviceAddress, functionNo, registerAddress, registerData, TestContext.Current.CancellationToken);
        var response = await _givEnergyClient.WaitForResponseAsync(TestContext.Current.CancellationToken);

        _givEnergyClient.Close();

        response.SerialNumber.ShouldBe(SerialNo);
        response.WifiAdapter.ShouldBe(WifiHost);
        response.DeviceNumber.ShouldBe((byte)(deviceIndex + 1));
        response.ResponseDataType.ShouldBe(responseDataType);
        response.ResponseData.ShouldBeOfType(expected.GetType());
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
        var expectedRequestData = new byte[] { deviceAddress, functionNo, (byte)(registerAddress >> 8), (byte)(registerAddress & 0xFF), 0, 60 };
        var checkSumData = _checkSumService.CheckSum(expectedRequestData);
        byte[] expectedRequest = [89, 89, 0, 1, 0, 28, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, .. expectedRequestData, .. checkSumData];
        byte[] bufferCheck = [.. expectedRequest, .. Enumerable.Range(1, 1024 - expectedRequest.Length).Select(_ => (byte)0)];
        buffer.ShouldBeEquivalentTo(bufferCheck);
    }

    private async Task SendResponseAsync(System.Net.Sockets.Socket inverterSocket, byte deviceAddress, byte functionNo, ushort registerAddress, IEnumerable<byte> registerData, CancellationToken cancellationToken)
    {
        byte[] pduData = [deviceAddress, functionNo, .. SerialNo.Select(x => (byte)x), (byte)(registerAddress >> 8), (byte)(registerAddress & 0xFF), 0, 60, .. registerData];
        var checkSumData = _checkSumService.CheckSum(pduData);
        byte[] responseData = [89, 89, 0, 1, 0, (byte)(pduData.Length + 22), 1, 2, .. WifiHost.Select(x => (byte)x), 0, 0, 0, 0, 0, 0, 0, (byte)(pduData.Length + 2), .. pduData, .. checkSumData];
        await inverterSocket.SendAsync(responseData, TestContext.Current.CancellationToken);
    }
}
