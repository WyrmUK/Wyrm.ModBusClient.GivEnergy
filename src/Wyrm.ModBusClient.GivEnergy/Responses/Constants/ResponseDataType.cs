namespace Wyrm.ModBusClient.GivEnergy.Responses.Constants;

/// <summary>
/// The specific response data types that can be returned.
/// </summary>
public enum ResponseDataType
{
    /// <summary>
    /// Undecoded register data.
    /// </summary>
    RegisterData,
    /// <summary>
    /// Battery data block 2.
    /// </summary>
    BatteryData2 = 2,
    /// <summary>
    /// Meter data block 2.
    /// </summary>
    MeterData2 = 12,
    /// <summary>
    /// Low voltage BCU data block 2.
    /// </summary>
    LowVoltageBCUData2 = 22,
    /// <summary>
    /// Inverter data block 1.
    /// </summary>
    InverterData1 = 101,
    /// <summary>
    /// Inverter data block 5.
    /// </summary>
    InverterData5 = 105,
    /// <summary>
    /// Inverter properties block 1.
    /// </summary>
    InverterProperties1 = 201,
    /// <summary>
    /// Inverter properties block 2.
    /// </summary>
    InverterProperties2,
    /// <summary>
    /// Inverter properties block 3.
    /// </summary>
    InverterProperties3,
    /// <summary>
    /// Inverter properties block 4.
    /// </summary>
    InverterProperties4,
    /// <summary>
    /// Inverter properties block 5.
    /// </summary>
    InverterProperties5,
    /// <summary>
    /// Inverter properties block 6.
    /// </summary>
    InverterProperties6,
    /// <summary>
    /// Inverter properties block 9.
    /// </summary>
    InverterProperties9 = 209,
    /// <summary>
    /// Inverter properties block 10.
    /// </summary>
    InverterProperties10
}
