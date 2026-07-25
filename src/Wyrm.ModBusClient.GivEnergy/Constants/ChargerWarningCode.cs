namespace Wyrm.ModBusClient.GivEnergy.Constants;

/// <summary>
/// Warning codes for the charger and battery.
/// </summary>
public enum ChargerWarningCode : ushort
{
    /// <summary>
    /// BMS Under Temperature (charge).
    /// </summary>
    BMSUnderTemperatureCharge = 0x0001,
    /// <summary>
    /// BMS Under Temperature (discharge).
    /// </summary>
    BMSUnderTemperatureDischarge = 0x0002,
    /// <summary>
    /// BMS Over Temperature (charge).
    /// </summary>
    BMSOverTemperatureCharge = 0x0004,
    /// <summary>
    /// BMS Over Temperature (discharge).
    /// </summary>
    BMSOverTemperatureDischarge = 0x0008,
    /// <summary>
    /// BMS Under Voltage.
    /// </summary>
    BMSUnderVoltage = 0x0010,
    /// <summary>
    /// BMS Over Voltage.
    /// </summary>
    BMSOverVoltage = 0x0020,
    /// <summary>
    /// BMS Short Circuit Current (charge).
    /// </summary>
    BMSShortCircuitCurrentCharge = 0x0040,
    /// <summary>
    /// BMS OVEr Current (discharge).
    /// </summary>
    BMSOverCurrentDischarge = 0x0080,
    /// <summary>
    /// Charge/Discharge Module Temperature Fault.
    /// </summary>
    ChargeDischargeModuleTemperatureFault = 0x0100,
    /// <summary>
    /// Battery Temperature Fault.
    /// </summary>
    BatteryTemperatureFault = 0x0200,
    /// <summary>
    /// BMS Comms Fail.
    /// </summary>
    BMSCommsFail = 0x0400,
    /// <summary>
    /// Reserved.
    /// </summary>
    Reserved = 0x0800,
    /// <summary>
    /// Battery Soft-Start Fail.
    /// </summary>
    BatterySoftStartFail = 0x1000,
    /// <summary>
    /// Battery Voltage Low.
    /// </summary>
    BatteryVoltageLow = 0x2000,
    /// <summary>
    /// Battery Voltage High.
    /// </summary>
    BatteryVoltageHigh = 0x4000,
    /// <summary>
    /// Electricity Meter Comms Fail.
    /// </summary>
    ElectricityMeterCommsFail = 0x8000
}
