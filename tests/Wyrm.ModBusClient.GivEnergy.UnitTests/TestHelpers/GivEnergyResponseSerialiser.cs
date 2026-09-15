using System.Text.Json;
using Wyrm.ModBusClient.GivEnergy.Responses;
using Wyrm.ModBusClient.GivEnergy.Responses.Constants;
using Xunit.Sdk;

namespace Wyrm.ModBusClient.GivEnergy.UnitTests.TestHelpers;

public class GivEnergyResponseSerialiser : XunitSerializer<GivEnergyResponse>
{
    public override string Serialize(GivEnergyResponse value)
    {
        var responseData = JsonSerializer.Serialize(value.ResponseData);
        return $"{value.SerialNumber},{value.WifiAdapter},{value.DeviceNumber},{value.ResponseDataType},{responseData}";
    }

    public override GivEnergyResponse Deserialize(Type type, string serializedValue)
    {
        var values = serializedValue.Split([','], 5);
        var responseDataType = Enum.Parse<ResponseDataType>(values[3]);
        return new GivEnergyResponse
        {
            SerialNumber = values[0],
            WifiAdapter = values[1],
            DeviceNumber = byte.Parse(values[2]),
            ResponseDataType = responseDataType,
            ResponseData = responseDataType switch
            {
                ResponseDataType.BatteryData2 => JsonSerializer.Deserialize<BatteryData2>(values[4])!,
                ResponseDataType.MeterData2 => JsonSerializer.Deserialize<MeterData2>(values[4])!,
                ResponseDataType.LowVoltageBCUData2 => JsonSerializer.Deserialize<LowVoltageBCUData2>(values[4])!,
                ResponseDataType.InverterData1 => JsonSerializer.Deserialize<InverterData1>(values[4])!,
                ResponseDataType.InverterData5 => JsonSerializer.Deserialize<InverterData5>(values[4])!,
                ResponseDataType.InverterProperties1 => JsonSerializer.Deserialize<InverterProperties1>(values[4])!,
                ResponseDataType.InverterProperties2 => JsonSerializer.Deserialize<InverterProperties2>(values[4])!,
                ResponseDataType.InverterProperties3 => JsonSerializer.Deserialize<InverterProperties3>(values[4])!,
                ResponseDataType.InverterProperties4 => JsonSerializer.Deserialize<InverterProperties4>(values[4])!,
                ResponseDataType.InverterProperties5 => JsonSerializer.Deserialize<InverterProperties5>(values[4])!,
                ResponseDataType.InverterProperties6 => JsonSerializer.Deserialize<InverterProperties6>(values[4])!,
                ResponseDataType.InverterProperties9 => JsonSerializer.Deserialize<InverterProperties9>(values[4])!,
                ResponseDataType.InverterProperties10 => JsonSerializer.Deserialize<InverterProperties10>(values[4])!,
                _ => JsonSerializer.Deserialize<RegisterData>(values[4])!
            }
        };
    }
}
