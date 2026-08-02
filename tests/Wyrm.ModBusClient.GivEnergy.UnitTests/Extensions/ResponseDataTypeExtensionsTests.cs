using Shouldly;
using Wyrm.ModBusClient.GivEnergy.Extensions;
using Wyrm.ModBusClient.GivEnergy.Responses.Constants;
using Wyrm.ModBusClient.GivEnergy.Services;

namespace Wyrm.ModBusClient.GivEnergy.UnitTests.Extensions;

public class ResponseDataTypeExtensionsTests
{
    public static TheoryData<ResponseDataType, byte, byte> DeviceAddressTests()
    {
        var data = new TheoryData<ResponseDataType, byte, byte>();
        foreach (var type in Enum.GetValues<ResponseDataType>())
        {
            switch (type)
            {
                case ResponseDataType.RegisterData:
                    break;
                case ResponseDataType.BatteryData2:
                    for (byte index = 0; index <= (InverterDataConverter.BatteryMax - InverterDataConverter.BatteryMin); ++index)
                    {
                        data.Add(type, index, (byte)(InverterDataConverter.BatteryMin + index));
                    }
                    break;
                case ResponseDataType.MeterData2:
                    for (byte index = 0; index <= (InverterDataConverter.MeterMax - InverterDataConverter.MeterMin); ++index)
                    {
                        data.Add(type, index, (byte)(InverterDataConverter.MeterMin + index));
                    }
                    break;
                case ResponseDataType.LowVoltageBCUData2:
                    data.Add(type, 0, InverterDataConverter.LowVoltageBCUId);
                    break;
                default:
                    data.Add(type, 0, InverterDataConverter.InverterId);
                    break;
            }
        }
        return data;
    }

    [Theory, MemberData(nameof(DeviceAddressTests))]
    public void DeviceAddress_Should_Get_Correct_Address(ResponseDataType responseDataType, byte index, byte expected)
    {
        var result = responseDataType.DeviceAddress(index);

        result.ShouldBe(expected);
    }

    public static TheoryData<ResponseDataType, byte> DeviceAddressExceptions()
    {
        var data = new TheoryData<ResponseDataType, byte>();
        foreach (var type in Enum.GetValues<ResponseDataType>())
        {
            switch (type)
            {
                case ResponseDataType.RegisterData:
                    data.Add(type, 0);
                    break;
                case ResponseDataType.BatteryData2:
                    data.Add(type, InverterDataConverter.BatteryMax - InverterDataConverter.BatteryMin + 1);
                    break;
                case ResponseDataType.MeterData2:
                    data.Add(type, InverterDataConverter.MeterMax - InverterDataConverter.MeterMin + 1);
                    break;
                case ResponseDataType.LowVoltageBCUData2:
                    data.Add(type, 1);
                    break;
                default:
                    data.Add(type, 1);
                    break;
            }
        }
        return data;
    }

    [Theory, MemberData(nameof(DeviceAddressExceptions))]
    public void DeviceAddress_Should_Throw_GivEnergyClientException_If_Unsupported_Combination(ResponseDataType responseDataType, byte index)
    {
        Should.Throw<GivEnergyClientException>(() => responseDataType.DeviceAddress(index));
    }

    [Theory, CombinatorialData]
    public void InputRegisters_Should_Return_True_For_Data(ResponseDataType responseDataType)
    {
        var result = responseDataType.InputRegisters();
        result.ShouldBe(responseDataType < ResponseDataType.InverterProperties1);
    }

    [Theory, CombinatorialData]
    public void StartAddress_should_Return_Correct_Value(ResponseDataType responseDataType)
    {
        if (responseDataType == ResponseDataType.RegisterData) return;

        var result = responseDataType.StartAddress();

        result.ShouldBe((ushort)(responseDataType switch
        {
            ResponseDataType.BatteryData2 => 60,
            ResponseDataType.MeterData2 => 60,
            ResponseDataType.LowVoltageBCUData2 => 60,
            ResponseDataType.InverterData5 => 240,
            ResponseDataType.InverterProperties2 => 60,
            ResponseDataType.InverterProperties3 => 120,
            ResponseDataType.InverterProperties4 => 180,
            ResponseDataType.InverterProperties5 => 240,
            ResponseDataType.InverterProperties6 => 300,
            ResponseDataType.InverterProperties9 => 480,
            ResponseDataType.InverterProperties10 => 540,
            _ => 0
        }));
    }

    [Fact]
    public void StartAddress_Should_Throw_GivEnergyClientException_For_Unsupported_Type()
    {
        Should.Throw<GivEnergyClientException>(() => ResponseDataType.RegisterData.StartAddress());
    }
}
