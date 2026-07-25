using Wyrm.ModBusClient.GivEnergy.Constants;

namespace Wyrm.ModBusClient.GivEnergy.Responses;

/// <summary>
/// Properties from the first data set from a GivEnergy inverter.
/// </summary>
public record InverterData1 : IResponseData
{
    /// <summary>
    /// Gets the status of the inverter.
    /// </summary>
    public GivEnergyStatus Status { get; init; }
    /// <summary>
    /// Gets the voltage from the first PV set.
    /// </summary>
    public decimal PV1Voltage { get; init; }
    /// <summary>
    /// Gets the voltage from the second PV set (V).
    /// </summary>
    public decimal PV2Voltage { get; init; }
    /// <summary>
    /// Gets the voltage for the internal P Bus (V).
    /// </summary>
    public decimal PBusVoltage { get; init; }
    /// <summary>
    /// Gets the voltage for the internal N Bus (V).
    /// </summary>
    public decimal NBusVoltage { get; init; }
    /// <summary>
    /// Gets the AC Grid voltage (V).
    /// </summary>
    public decimal GridVoltage { get; init; }
    /// <summary>
    /// Gets the battery throughput (?).
    /// </summary>
    public decimal BatteryThroughput { get; init; }
    /// <summary>
    /// Gets the input current for the first PV set (A).
    /// </summary>
    public decimal PV1InputCurrent { get; init; }
    /// <summary>
    /// Gets the input current for the second PV set (A).
    /// </summary>
    public decimal PV2InputCurrent { get; init; }
    /// <summary>
    /// Gets the AC Grid output current (A).
    /// </summary>
    public decimal GridOutputCurrent { get; init; }
    /// <summary>
    /// Gets the total generating capacity of the PV (W).
    /// </summary>
    public decimal PVGeneratingCapacityTotal { get; init; }
    /// <summary>
    /// Gets the frequency of the grid voltage (Hz).
    /// </summary>
    public decimal GridFrequency { get; init; }
    /// <summary>
    /// Gets the Charge Status (deprecated).
    /// </summary>
    public ushort ChargeStatus { get; init; }
    /// <summary>
    /// Gets the Charge Status Type.
    /// </summary>
    public ChargeStatus ChargeStatusType { get; init; }
    /// <summary>
    /// Gets the Highbrigh Bus Voltage (V).
    /// </summary>
    public decimal HighbrighBusVoltage { get; init; }
    /// <summary>
    /// Gets the Inverter Output Power Factor for now.
    /// </summary>
    public decimal InverterOutputPowerFactorNow { get; init; }
    /// <summary>
    /// Gets the Energy Today for the first PV set (kWh).
    /// </summary>
    public decimal PV1EnergyToday { get; init; }
    /// <summary>
    /// Gets the input power for the first PV set (W).
    /// </summary>
    public decimal PV1InputPower { get; init; }
    /// <summary>
    /// Gets the Energy Today for the second PV set (kWh).
    /// </summary>
    public decimal PV2EnergyToday { get; init; }
    /// <summary>
    /// Gets the input power for the second PV set (W).
    /// </summary>
    public decimal PV2InputPower { get; init; }
    /// <summary>
    /// Gets the output energy to the grid in total (kWh).
    /// </summary>
    public decimal GridOutEnergyTotal { get; init; }
    /// <summary>
    /// Gets the PV mate (Solar Diverter - kWh).
    /// </summary>
    public decimal PVSolarDiverterEnergy { get; init; }
    /// <summary>
    /// Gets the grid output power flow onto the busbar (+ve = delivering), not the actual grid out (W).
    /// </summary>
    public decimal GridPowerPH1 { get; init; }
    /// <summary>
    /// Gets the grid energy out for the day (kWh).
    /// </summary>
    public decimal GridOutEnergyToday { get; init; }
    /// <summary>
    /// Gets the grid energy in for the day (kWh).
    /// </summary>
    public decimal GridInEnergyToday { get; init; }
    /// <summary>
    /// Gets the inverter input energy total (kWh).
    /// </summary>
    public decimal InverterInEnergyTotal { get; init; }
    /// <summary>
    /// Gets the discharge energy year (?).
    /// </summary>
    public decimal DischargeEnergyYear { get; init; }
    /// <summary>
    /// Gets the power to/from (+ve/-ve) the grid at the meter via the clamp (W).
    /// </summary>
    public decimal GridPowerAtMeter { get; init; }
    /// <summary>
    /// Gets the backup power (W).
    /// </summary>
    public decimal BackupPower { get; init; }
    /// <summary>
    /// Gets the grid energy in total (kWh).
    /// </summary>
    public decimal GridInEnergyTotal { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg0034 { get; init; }
    /// <summary>
    /// Gets the AC Charge Energy today (kWh).
    /// </summary>
    public decimal ACChargeEnergyToday { get; init; }
    /// <summary>
    /// Gets the battery charge energy today (kWh).
    /// </summary>
    public decimal BatteryChargeEnergyTodayAlt1 { get; init; }
    /// <summary>
    /// Gets the battery discharge energy today (kWh).
    /// </summary>
    public decimal BatteryDischargeEnergyTodayAlt1 { get; init; }
    /// <summary>
    /// Gets the countdown value.
    /// </summary>
    public ushort Countdown { get; init; }
    /// <summary>
    /// Gets the inverter fault code.
    /// </summary>
    public string InverterFaultCode { get; init; } = "0000";
    /// <summary>
    /// Gets the inverter warning code.
    /// </summary>
    public string InverterWarningCode { get; init; } = "0000";
    /// <summary>
    /// Gets the inverter heatsink temperature (C).
    /// </summary>
    public decimal InverterHeatsinkTemperature { get; init; }
    /// <summary>
    /// Gets the load power demand (W).
    /// </summary>
    public decimal LoadPowerDemand { get; init; }
    /// <summary>
    /// Gets the apparent grid power PH1 (W).
    /// </summary>
    public decimal GridPowerApparent { get; init; }
    /// <summary>
    /// Gets the single phase PV generation energy today (W).
    /// </summary>
    public decimal PVGenerationEnergyToday { get; init; }
    /// <summary>
    /// Gets the single phase PV generation energy total (W).
    /// </summary>
    public decimal PVGenerationEnergyTotal { get; init; }
    /// <summary>
    /// Gets the time since first power on.
    /// </summary>
    public TimeSpan WorkTimeTotal { get; init; }
    /// <summary>
    /// Gets the system mode.
    /// </summary>
    public ushort SystemMode { get; init; }
    /// <summary>
    /// Gets the battery voltage (V).
    /// </summary>
    public decimal BatteryVoltage { get; init; }
    /// <summary>
    /// Gets the battery current (A).
    /// </summary>
    public decimal BatteryCurrent { get; init; }
    /// <summary>
    /// Gets the battery power (W).
    /// </summary>
    public decimal BatteryPower { get; init; }
    /// <summary>
    /// Gets the AC1 Output voltage (V).
    /// </summary>
    public decimal AC1OutputVoltage { get; init; }
    /// <summary>
    /// Gets the AC1 Output frequency (Hz).
    /// </summary>
    public decimal AC1OutputFrequency { get; init; }
    /// <summary>
    /// Gets the charger temperature (C).
    /// </summary>
    public decimal ChargerTemperature { get; init; }
    /// <summary>
    /// Gets the battery temperature (C).
    /// </summary>
    public decimal BatteryTemperature { get; init; }
    /// <summary>
    /// Gets the charger warning code.
    /// </summary>
    public ushort ChargerWarningCode { get; init; }
    /// <summary>
    /// Gets the charger warning code messages.
    /// </summary>
    public ICollection<ChargerWarningCode> ChargerWarningMessages { get; init; } = [];
    /// <summary>
    /// Gets the inverter AC grid-terminal current (A).
    /// </summary>
    public decimal GridPortCurrent { get; init; }
    /// <summary>
    /// Gets the battery state of charge (%).
    /// </summary>
    public decimal BatteryPercentage { get; init; }
}
