using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Net;
using System.Reflection;
using Wyrm.ModBusClient.GivEnergy.Responses;
using Wyrm.ModBusClient.GivEnergy.Responses.Constants;
using Wyrm.ModBusClient.GivEnergy.Services;

namespace Wyrm.ModBusClient.GivEnergy.UnitTests;

public class GivEnergyClientTests
{
    #region Setup

    private readonly IModBusRegisterClient _modBusRegisterClient = Mock.Of<IModBusRegisterClient>();
    private readonly IInverterDataConverter _inverterDataConverter = Mock.Of<IInverterDataConverter>();
    private readonly IFramerService _framerService = Mock.Of<IFramerService>();
    private readonly ILogger<GivEnergyClient> _logger = Mock.Of<ILogger<GivEnergyClient>>();

    private readonly IGivEnergyClient _givEnergyClient;

    private static readonly EndPoint TestEndPoint = new IPEndPoint(new IPAddress([127, 0, 0, 1]), 8000);
    private readonly CancellationToken _cancellationToken = TestContext.Current.CancellationToken;

    private const ushort ProtocolIdentifier = 0x0001;
    private const ushort TransactionId = 0x5959;
    private const ushort RegisterBlockCount = 60;

    public GivEnergyClientTests()
    {
        _givEnergyClient = new GivEnergyClient(
            _modBusRegisterClient,
            _inverterDataConverter,
            _framerService,
            _logger);
    }

    private static ModBusClientException ModBusClientException
    {
        get
        {
            var constructor = typeof(ModBusClientException)
                .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, [typeof(string), typeof(ModBusExceptionCode), typeof(Exception)]);
            return (ModBusClientException)constructor!.Invoke(["Test", ModBusExceptionCode.ConnectionError, null]);
        }
    }

    #endregion

    [Fact]
    public async Task ConnectAsync_Should_Call_ConnectAsync_And_Set_Values()
    {
        await _givEnergyClient.ConnectAsync(TestEndPoint, _cancellationToken);
        Mock.Get(_modBusRegisterClient)
            .VerifySet(x => x.ProtocolIdentifier = ProtocolIdentifier, Times.Once);
        Mock.Get(_modBusRegisterClient)
            .VerifySet(x => x.PduFramer = It.IsAny<Func<IList<byte>, IList<byte>>>(), Times.Once);
        Mock.Get(_modBusRegisterClient)
            .VerifySet(x => x.PduDeframer = It.IsAny< Func<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>>>(), Times.Once);
        Mock.Get(_modBusRegisterClient)
            .Verify(x => x.ConnectAsync(TestEndPoint, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task ConnectAsync_Should_Not_Suppress_ModBusClientException()
    {
        Mock.Get(_modBusRegisterClient)
            .Setup(x => x.ConnectAsync(It.IsAny<EndPoint>(), It.IsAny<CancellationToken>()))
            .Throws(ModBusClientException);
        await Should.ThrowAsync<ModBusClientException>(() => _givEnergyClient.ConnectAsync(TestEndPoint, _cancellationToken).AsTask());
    }

    public static readonly TheoryData<ResponseDataType, byte, byte, ushort, bool> RequestInverterDataTests = new()
    {
        { ResponseDataType.InverterProperties1, 0, InverterDataConverter.InverterId, 0, false },
        { ResponseDataType.BatteryData2, 1, 0x33, 60, true }
    };

    [Theory, MemberData(nameof(RequestInverterDataTests))]
    public async Task RequestInverterDataAsync_Should_Call_Correct_Request(ResponseDataType responseDataType, byte deviceIndex, byte expectedAddress, ushort expectedStartingRegister, bool inputRegisters)
    {
        await _givEnergyClient.RequestInverterDataAsync(responseDataType, deviceIndex, _cancellationToken);
        Mock.Get(_modBusRegisterClient)
            .VerifySet(x => x.TransactionId = TransactionId, Times.Once);
        Mock.Get(_modBusRegisterClient)
            .VerifySet(x => x.UnitIdentifier = expectedAddress, Times.Once);
        Mock.Get(_modBusRegisterClient)
            .Verify(x => x.ReadInputRegistersRequestAsync(expectedStartingRegister, RegisterBlockCount, _cancellationToken), inputRegisters ? Times.Once : Times.Never);
        Mock.Get(_modBusRegisterClient)
            .Verify(x => x.ReadHoldingRegistersRequestAsync(expectedStartingRegister, RegisterBlockCount, _cancellationToken), inputRegisters ? Times.Never : Times.Once);
    }

    [Fact]
    public async Task RequestInverterDataAsync_Should_Not_Suppress_ModBusClientException()
    {
        Mock.Get(_modBusRegisterClient)
            .Setup(x => x.ReadHoldingRegistersRequestAsync(0, RegisterBlockCount, _cancellationToken))
            .Throws(ModBusClientException);
        await Should.ThrowAsync<ModBusClientException>(() => _givEnergyClient.RequestInverterDataAsync(cancellationToken: _cancellationToken).AsTask());
    }

    public static readonly TheoryData<byte, bool, ushort> SendReadRegistersTests = new()
    {
        { 0x11, false, 0 },
        { 0x33, true, 60 }
    };

    [Theory, MemberData(nameof(SendReadRegistersTests))]
    public async Task SendReadRegistersAsync_Should_Call_Correct_Request(byte deviceAddress, bool inputRegisters, ushort startAddress)
    {
        await _givEnergyClient.SendReadRegistersAsync(deviceAddress, inputRegisters, startAddress, _cancellationToken);
        Mock.Get(_modBusRegisterClient)
            .VerifySet(x => x.TransactionId = TransactionId, Times.Once);
        Mock.Get(_modBusRegisterClient)
            .VerifySet(x => x.UnitIdentifier = deviceAddress, Times.Once);
        Mock.Get(_modBusRegisterClient)
            .Verify(x => x.ReadInputRegistersRequestAsync(startAddress, RegisterBlockCount, _cancellationToken), inputRegisters ? Times.Once : Times.Never);
        Mock.Get(_modBusRegisterClient)
            .Verify(x => x.ReadHoldingRegistersRequestAsync(startAddress, RegisterBlockCount, _cancellationToken), inputRegisters ? Times.Never : Times.Once);
    }

    [Fact]
    public async Task SendReadRegistersAsync_Should_Not_Suppress_ModBusClientException()
    {
        Mock.Get(_modBusRegisterClient)
            .Setup(x => x.ReadHoldingRegistersRequestAsync(0, RegisterBlockCount, _cancellationToken))
            .Throws(ModBusClientException);
        await Should.ThrowAsync<ModBusClientException>(() => _givEnergyClient.SendReadRegistersAsync(cancellationToken: _cancellationToken).AsTask());
    }

    private static readonly UshortDataResponse ReadRegistersResponse = new()
    {
        TransactionId = TransactionId,
        UnitIdentifier = InverterDataConverter.InverterId,
        FunctionNumber = 3,
        UshortData = [.. Enumerable.Range(1, 60).Select(x => (ushort)x)]
    };
    private const string WifiHost = "WH12345678";
    private const string SerialNo = "SN12345678";
    private const int RegisterAddress = 60;
    private static readonly GivEnergyResponse WaitForRegistersResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterData1,
        ResponseData = new InverterData1()
    };

    [Fact]
    public async Task WaitForResponseAsync_Should_Return_GivEnergyResponse()
    {
        Mock.Get(_modBusRegisterClient)
            .Setup(x => x.ReadRegistersResponseDataAsync(_cancellationToken))
            .ReturnsAsync(ReadRegistersResponse);
        Mock.Get(_framerService)
            .Setup(x => x.WifiHost)
            .Returns(WifiHost);
        Mock.Get(_framerService)
            .Setup(x => x.SerialNo)
            .Returns(SerialNo);
        Mock.Get(_framerService)
            .Setup(x => x.RegisterAddress)
            .Returns(RegisterAddress);
        Mock.Get(_inverterDataConverter)
            .Setup(x => x.ParseResponse(SerialNo, WifiHost, RegisterAddress, ReadRegistersResponse))
            .Returns(WaitForRegistersResponse);
        var result = await _givEnergyClient.WaitForResponseAsync(_cancellationToken);
        result.ShouldBe(WaitForRegistersResponse);
    }

    [Fact]
    public async Task WaitForResponseAsync_Should_Not_Suppress_ModBusClientException()
    {
        Mock.Get(_modBusRegisterClient)
            .Setup(x => x.ReadRegistersResponseDataAsync(_cancellationToken))
            .Throws(ModBusClientException);
        await Should.ThrowAsync<ModBusClientException>(() => _givEnergyClient.WaitForResponseAsync(_cancellationToken).AsTask());
    }

    private static readonly ReadOnlyMemory<byte> Pdu = new([1, 2, 3, 4, 5, 6, 7, 8]);

    [Fact]
    public async Task SendCustomPduAsync_Should_Call_SendCustomPduAsync_And_Set_Values()
    {
        await _givEnergyClient.SendCustomPduAsync(Pdu, _cancellationToken);
        Mock.Get(_modBusRegisterClient)
            .Verify(x => x.SendCustomPduAsync(Pdu, _cancellationToken), Times.Once);
        Mock.Get(_modBusRegisterClient)
            .VerifySet(x => x.TransactionId = TransactionId, Times.Once);
    }

    [Fact]
    public async Task SendCustomPduAsync_Should_Not_Suppress_ModBusClientException()
    {
        Mock.Get(_modBusRegisterClient)
            .Setup(x => x.SendCustomPduAsync(It.IsAny<ReadOnlyMemory<byte>>(), _cancellationToken))
            .Throws(ModBusClientException);
        await Should.ThrowAsync<ModBusClientException>(() => _givEnergyClient.SendCustomPduAsync(Pdu, _cancellationToken).AsTask());
    }

    [Fact]
    public void Close_Should_Call_Close()
    {
        _givEnergyClient.Close();
        Mock.Get(_modBusRegisterClient)
            .Verify(x => x.Close(), Times.Once);
    }
}
