using Wyrm.ModBusClient.GivEnergy.Responses.Constants;
using Wyrm.ModBusClient.GivEnergy.Services;

namespace Wyrm.ModBusClient.GivEnergy.Extensions;

internal static class ResponseDataTypeExtensions
{
    extension (ResponseDataType value)
    {
        public byte DeviceAddress(byte index) => value switch
        {
            ResponseDataType.BatteryData2 when index <= (InverterDataConverter.BatteryMax - InverterDataConverter.BatteryMin) => (byte)(InverterDataConverter.BatteryMin + index),
            ResponseDataType.MeterData2 when index <= (InverterDataConverter.MeterMax - InverterDataConverter.MeterMin) => (byte)(InverterDataConverter.MeterMin + index),
            ResponseDataType.LowVoltageBCUData2 when index == 0 => (byte)(InverterDataConverter.LowVoltageBCUId + index),
            >= ResponseDataType.InverterData1 when index == 0 => (byte)(InverterDataConverter.InverterId + index),
            _ => throw new GivEnergyClientException("Unsupported device address.")
        };

        public bool InputRegisters() => value switch
        {
            >= ResponseDataType.InverterProperties1 => false,
            _ => true
        };

        public ushort StartAddress() => value switch
        {
            ResponseDataType.InverterData1 or ResponseDataType.InverterProperties1 => 0,
            ResponseDataType.BatteryData2 or ResponseDataType.MeterData2 or ResponseDataType.LowVoltageBCUData2 or ResponseDataType.InverterProperties2 => 60,
            ResponseDataType.InverterProperties3 => 120,
            ResponseDataType.InverterProperties4 => 180,
            ResponseDataType.InverterProperties5 => 240,
            ResponseDataType.InverterProperties6 => 300,
            ResponseDataType.InverterProperties9 => 480,
            ResponseDataType.InverterProperties10 => 540,
            _ => throw new GivEnergyClientException("Unsupported start address.")
        };
    }
}
