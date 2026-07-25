namespace Wyrm.ModBusClient.GivEnergy.Constants;

/// <summary>
/// Inverter fault codes.
/// </summary>
public enum InverterFaultCode
{
    /// <summary>
    /// Backup overload fault.
    /// </summary>
    BackupOverloadFault = 3,
    /// <summary>
    /// Grid monitor comm fault.
    /// </summary>
    GridMonitorCommFault = 6,
    /// <summary>
    /// ARM comms fault.
    /// </summary>
    ARMCommsFault = 7,
    /// <summary>
    /// Consistent fault.
    /// </summary>
    ConsistentFault = 8,
    /// <summary>
    /// EEPROM fault.
    /// </summary>
    EEPROMFault = 9,
    /// <summary>
    /// Inverter frequency fault.
    /// </summary>
    InverterFrequencyFault = 16,
    /// <summary>
    /// Relay fault.
    /// </summary>
    RelayFault = 17,
    /// <summary>
    /// Inverter voltage fault.
    /// </summary>
    InverterVoltageFault = 18,
    /// <summary>
    /// GFCI fault.
    /// </summary>
    GFCIFault = 19,
    /// <summary>
    /// Hall sensor fault.
    /// </summary>
    HallSensorFault = 20,
    /// <summary>
    /// DSP comms fault.
    /// </summary>
    DSPCommsFault = 21,
    /// <summary>
    /// Bus over voltage.
    /// </summary>
    BusOverVoltage = 22,
    /// <summary>
    /// Inverter current fault.
    /// </summary>
    InverterCurrentFault = 23,
    /// <summary>
    /// No utility.
    /// </summary>
    NoUtility = 24,
    /// <summary>
    /// PV isolation fault.
    /// </summary>
    PVIsolationFault = 25,
    /// <summary>
    /// Current leak high.
    /// </summary>
    CurrentLeakHigh = 26,
    /// <summary>
    /// DCI high.
    /// </summary>
    DCIHigh = 27,
    /// <summary>
    /// PV over voltage.
    /// </summary>
    PVOverVoltage = 28,
    /// <summary>
    /// Grid voltage fault.
    /// </summary>
    GridVoltageFault = 29,
    /// <summary>
    /// Grid frequency fault.
    /// </summary>
    GridFrequencyFault = 30,
    /// <summary>
    /// Inverter NTC fault.
    /// </summary>
    InverterNTCFault = 31
}
