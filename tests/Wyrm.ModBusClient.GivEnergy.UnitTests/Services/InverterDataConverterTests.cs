using Shouldly;
using Wyrm.ModBusClient.GivEnergy.Responses;
using Wyrm.ModBusClient.GivEnergy.Responses.Constants;
using Wyrm.ModBusClient.GivEnergy.Responses.Models;
using Wyrm.ModBusClient.GivEnergy.Services;

namespace Wyrm.ModBusClient.GivEnergy.UnitTests.Services;

public class InverterDataConverterTests
{
    #region Setup

    private readonly IInverterDataConverter _inverterDataConverter = new InverterDataConverter();

    private const string SerialNo = "ID12345678";
    private const string WifiHost = "WH12345678";
    private const ushort TransactionId = 0x5959;
    private const byte HoldingRegisters = 0x03;
    private const byte InputRegisters = 0x04;

    #endregion

    [Theory, CombinatorialData]
    public void ParseResponse_Should_Throw_GivEnergyClientException_If_Not_60_Registers([CombinatorialValues(0, 1, 59, 61)] int numRegisters)
    {
        var response = new UshortDataResponse
        {
            TransactionId = TransactionId,
            UnitIdentifier = InverterDataConverter.InverterId,
            FunctionNumber = HoldingRegisters,
            UshortData = new ushort[numRegisters]
        };
        Should.Throw<GivEnergyClientException>(() => _inverterDataConverter.ParseResponse(SerialNo, WifiHost, 0, response));
    }

    private static readonly ushort[] ZeroData = new ushort[60];

    private static readonly GivEnergyResponse InverterProperties1ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties1,
        ResponseData = new InverterProperties1
        {
            DeviceTypeCode = "0000",
            Module = "00000000",
            ModbusVersion = "0.00",
            PowerFactor = -1M
        }
    };

    private static readonly GivEnergyResponse InverterProperties2ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties2,
        ResponseData = new InverterProperties2()
    };

    private static readonly GivEnergyResponse InverterProperties3ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties3,
        ResponseData = new InverterProperties3
        {
            PowerFactorPoints = new (ushort, ushort)[4],
            LVFRTLowFaults = new (ushort, ushort)[4],
            LVFRTHighFaults = new (ushort, ushort)[1]
        }
    };

    private static readonly GivEnergyResponse InverterProperties4ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties4,
        ResponseData = new InverterProperties4
        {
            Reg181To199 = new ushort[19],
            Reg226To240 = new ushort[35]
        }
    };

    private static readonly GivEnergyResponse InverterProperties5ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties5,
        ResponseData = new InverterProperties5()
    };

    private static readonly GivEnergyResponse InverterProperties6ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties6,
        ResponseData = new InverterProperties6()
    };

    private static readonly GivEnergyResponse InverterProperties9ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties9,
        ResponseData = new InverterProperties9
        {
            Reg481To499 = new ushort[19],
            Reg512To540 = new ushort[30]
        }
    };

    private static readonly GivEnergyResponse InverterProperties10ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties10,
        ResponseData = new InverterProperties10
        {
            Reg541To554 = new ushort[14],
            Reg575To600 = new ushort[27]
        }
    };

    private static readonly GivEnergyResponse InverterData1ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterData1,
        ResponseData = new InverterData1
        {
            InverterOutputPowerFactorNow = -1M
        }
    };

    private static readonly GivEnergyResponse InverterData5ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterData5,
        ResponseData = new InverterData5()
    };

    private static GivEnergyResponse MeterData2ZeroResponse(byte deviceNumber) => new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = deviceNumber,
        ResponseDataType = ResponseDataType.MeterData2,
        ResponseData = new MeterData2
        {
            Phase1 = new MeterPhaseData { PowerFactor = -1M },
            Phase2 = new MeterPhaseData { PowerFactor = -1M },
            Phase3 = new MeterPhaseData { PowerFactor = -1M },
            TotalPowerFactor = -1M
        }
    };

    private static readonly GivEnergyResponse LowVoltageBCUData2ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.LowVoltageBCUData2,
        ResponseData = new LowVoltageBCUData2()
    };

    private static GivEnergyResponse BatteryData2ZeroResponse(byte deviceNumber) => new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = deviceNumber,
        ResponseDataType = ResponseDataType.BatteryData2,
        ResponseData = new BatteryData2
        {
            CellVoltages = new decimal[16],
            CellsTemperature = new decimal[4]
        }
    };

    private static readonly ushort[] UniqueData = Enumerable.Range(0x0020, 60).Select(x => (ushort)x).ToArray();

    public static TheoryData<byte, byte, ushort[], int, GivEnergyResponse> ParseTests()
    {
        var data = new TheoryData<byte, byte, ushort[], int, GivEnergyResponse>
        {
            { InverterDataConverter.InverterId, HoldingRegisters, ZeroData, 0, InverterProperties1ZeroResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ZeroData, 60, InverterProperties2ZeroResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ZeroData, 120, InverterProperties3ZeroResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ZeroData, 180, InverterProperties4ZeroResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ZeroData, 240, InverterProperties5ZeroResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ZeroData, 300, InverterProperties6ZeroResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ZeroData, 480, InverterProperties9ZeroResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ZeroData, 540, InverterProperties10ZeroResponse },
            { InverterDataConverter.InverterId, InputRegisters, ZeroData, 0, InverterData1ZeroResponse },
            { InverterDataConverter.InverterId, InputRegisters, ZeroData, 240, InverterData5ZeroResponse }
        };
        for (byte meterNum = 0; meterNum < InverterDataConverter.MeterMax - InverterDataConverter.MeterMin; ++meterNum)
        {
            data.Add((byte)(InverterDataConverter.MeterMin + meterNum), InputRegisters, ZeroData, 60, MeterData2ZeroResponse((byte)(meterNum + 1)));
        }
        data.Add(InverterDataConverter.LowVoltageBCUId, InputRegisters, ZeroData, 60, LowVoltageBCUData2ZeroResponse);
        for (byte batteryNum = 0; batteryNum < InverterDataConverter.BatteryMax - InverterDataConverter.BatteryMin; ++batteryNum)
        {
            data.Add((byte)(InverterDataConverter.BatteryMin + batteryNum), InputRegisters, ZeroData, 60, BatteryData2ZeroResponse((byte)(batteryNum + 1)));
        }
        return data;
    }

    [Theory, MemberData(nameof(ParseTests))]
    public void ParseResponse_Should_Parse_Data(byte unitIdentifier, byte functionNumber, ushort[] data, int address, GivEnergyResponse expected)
    {
        var response = new UshortDataResponse
        {
            TransactionId = TransactionId,
            UnitIdentifier = unitIdentifier,
            FunctionNumber = functionNumber,
            UshortData = data
        };
        var result = _inverterDataConverter.ParseResponse(SerialNo, WifiHost, address, response);
        result.SerialNumber.ShouldBe(expected.SerialNumber);
        result.WifiAdapter.ShouldBe(expected.WifiAdapter);
        result.DeviceNumber.ShouldBe(expected.DeviceNumber);
        result.ResponseDataType.ShouldBe(expected.ResponseDataType);
        result.ResponseData.ShouldBeEquivalentTo(expected.ResponseData);
    }

    private static readonly RegisterData RegisterDataResponse = new()
    {
        UnitIdentifier = 0x30,
        FunctionNumber = InputRegisters,
        StartAddress = 60,
        RegisterValues = [.. UniqueData]
    };

    [Fact]
    public void ParseResponse_Should_Parse_RegisterData()
    {
        var response = new UshortDataResponse
        {
            TransactionId = TransactionId,
            UnitIdentifier = RegisterDataResponse.UnitIdentifier,
            FunctionNumber = RegisterDataResponse.FunctionNumber,
            UshortData = UniqueData
        };
        var result = _inverterDataConverter.ParseResponse(SerialNo, WifiHost, RegisterDataResponse.StartAddress, response);
        result.SerialNumber.ShouldBe(SerialNo);
        result.WifiAdapter.ShouldBe(WifiHost);
        result.DeviceNumber.ShouldBe((byte)0);
        result.ResponseDataType.ShouldBe(ResponseDataType.RegisterData);
        ((RegisterData)result.ResponseData).UnitIdentifier.ShouldBe(RegisterDataResponse.UnitIdentifier);
        ((RegisterData)result.ResponseData).FunctionNumber.ShouldBe(RegisterDataResponse.FunctionNumber);
        ((RegisterData)result.ResponseData).StartAddress.ShouldBe(RegisterDataResponse.StartAddress);
        ((RegisterData)result.ResponseData).RegisterValues.ToArray().ShouldBeEquivalentTo(RegisterDataResponse.RegisterValues.ToArray());
    }
}
