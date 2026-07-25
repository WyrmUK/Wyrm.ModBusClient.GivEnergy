namespace Wyrm.ModBusClient.GivEnergy.Responses;

/// <summary>
/// Properties from the sixth input data set from a GivEnergy inverter.
/// Polled only when has_ac_config_block is set (AC-coupled and All-in-One models).
/// </summary>
public record InverterProperties6 : IResponseData
{
    /// <summary>
    /// Gets whether plant mode is enabled.
    /// </summary>
    public bool EnablePlantMode { get; init; }
    /// <summary>
    /// Gets plant role (?).
    /// </summary>
    public ushort PlantRole { get; init; }
    /// <summary>
    /// Gets plant meters (?).
    /// </summary>
    public ushort PlantMeters { get; init; }
    /// <summary>
    /// Gets thr over-frequency load drop recovery delay (?).
    /// </summary>
    public ushort OverFrequencyLoadDropRecoveryDelay { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg305 { get; init; }
    /// <summary>
    /// Gets the MPPT operating mode (?).
    /// </summary>
    public ushort MPPTOperatingMode { get; init; }
    /// <summary>
    /// Gets the connection loading slope (?).
    /// </summary>
    public ushort ConnectionLoadingSlope { get; init; }
    /// <summary>
    /// Gets the EPS nominal voltage (V).
    /// </summary>
    public decimal EPSNominalVoltage { get; init; }
    /// <summary>
    /// Gets the battery nominal power (W).
    /// </summary>
    public decimal BatteryNominalPower { get; init; }
    /// <summary>
    /// Gets the battery nominal current (A).
    /// </summary>
    public decimal BatteryNominalCurrent { get; init; }
    /// <summary>
    /// Gets the battery max charge percentage (%).
    /// </summary>
    public decimal BatteryMaxChargePercentage { get; init; }
    /// <summary>
    /// Gets the export priority (?).
    /// </summary>
    public ushort ExportPriority { get; init; }
    /// <summary>
    /// Gets the under-frequency add load delay (?).
    /// </summary>
    public ushort UnderFrequencyAddLoadDelay { get; init; }
    /// <summary>
    /// Gets the battery charge limit AC (?).
    /// </summary>
    public ushort BatteryChargeLimitAC { get; init; }
    /// <summary>
    /// Gets the battery discharge limit AC (?).
    /// </summary>
    public ushort BatteryDischargeLimitAC { get; init; }
    /// <summary>
    /// Gets the EN50549 zero current lower voltage limit (V).
    /// </summary>
    public decimal EN50549ZeroCurrentLowerVoltageLimit { get; init; }
    /// <summary>
    /// Gets the EN50549 zero current upper voltage limit (V).
    /// </summary>
    public decimal EN50549ZeroCurrentUpperVoltageLimit { get; init; }
    /// <summary>
    /// Gets whether EPS is enabled.
    /// </summary>
    public bool EnableEPS { get; init; }
    /// <summary>
    /// Gets the battery pause mode (?).
    /// </summary>
    public ushort BatteryPauseMode { get; init; }
    /// <summary>
    /// Gets the battery pause slot 1 time slot.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? BatteryPauseSlot1 { get; init; }
    /// <summary>
    /// Gets the over-frequency derating start point (?).
    /// </summary>
    public decimal OverFrequencyDeratingStartPoint { get; init; }
    /// <summary>
    /// Gets whether the tariff pricing battery logic is enabled.
    /// </summary>
    public bool EnableTariffPricingBatteryLogic { get; init; }
    /// <summary>
    /// Gets the import price battery discharge threshold (?).
    /// </summary>
    public ushort ImportPriceBatteryDischargeThreshold { get; init; }
    /// <summary>
    /// Gets the import price battery charge threshold (?).
    /// </summary>
    public ushort ImportPriceBatteryChargeThreshold { get; init; }
    /// <summary>
    /// Gets the export price battery discharge threshold (?).
    /// </summary>
    public ushort ExportPriceBatteryDischargeThreshold { get; init; }
    /// <summary>
    /// Gets the under-frequency derating start point (?).
    /// </summary>
    public decimal UnderFrequencyDeratingStartPoint { get; init; }
    /// <summary>
    /// Gets the under-frequency loading slope (?).
    /// </summary>
    public ushort UnderFrequencyLoadingSlope { get; init; }
    /// <summary>
    /// Gets the over-frequency derating stop point (?).
    /// </summary>
    public decimal OverFrequencyDeratingStopPoint { get; init; }
    /// <summary>
    /// Gets whether the BMS OCV calibration is enabled.
    /// </summary>
    public bool EnableBMSOCVCalibration { get; init; }
    /// <summary>
    /// Gets the gateway power off setting (?).
    /// </summary>
    public ushort GatewayPowerOffSetting { get; init; }
    /// <summary>
    /// Gets whether to force off grid.
    /// </summary>
    public bool ForceOffGrid { get; init; }
    /// <summary>
    /// Gets whether the micro grid is enabled.
    /// </summary>
    public bool EnableMicroGrid { get; init; }
    /// <summary>
    /// Gets whether the EV charger is enabled.
    /// </summary>
    public bool EnableEVCharger { get; init; }
    /// <summary>
    /// Gets the EV charger import limit (?).
    /// </summary>
    public ushort EVChargerImportLimit { get; init; }
    /// <summary>
    /// Gets the EV charger reconnection wait time (?).
    /// </summary>
    public ushort EVChargerReconnectionWaitTime { get; init; }
    /// <summary>
    /// Gets the EV charger SOC limit (?).
    /// </summary>
    public ushort EVChargerStateOfChargeLimit { get; init; }
    /// <summary>
    /// Gets whether the fan is enabled.
    /// </summary>
    public bool EnableFan { get; init; }
    /// <summary>
    /// Gets the fan speed (?).
    /// </summary>
    public ushort FanSpeed { get; init; }
    /// <summary>
    /// Gets whether the gateway is enabled.
    /// </summary>
    public bool EnableGateway { get; init; }
    /// <summary>
    /// Gets the BMC communication mode (?).
    /// </summary>
    public ushort BMSCommunicationMode { get; init; }
    /// <summary>
    /// Gets the N PE relay toggle (?).
    /// </summary>
    public ushort NPERelayToggle { get; init; }
    /// <summary>
    /// Gets the AFCI setting (?).
    /// </summary>
    public ushort AFCISetting { get; init; }
    /// <summary>
    /// Gets whether the generator is enabled.
    /// </summary>
    public bool EnableGenerator { get; init; }
    /// <summary>
    /// Gets the generator start SOC (?).
    /// </summary>
    public ushort GeneratorStartStateOfCharge { get; init; }
    /// <summary>
    /// Gets the generator stop SOC (?).
    /// </summary>
    public ushort GeneratorStopStateOfCharge { get; init; }
    /// <summary>
    /// Gets the generator charge power (W).
    /// </summary>
    public ushort GeneratorChargePower { get; init; }
    /// <summary>
    /// Gets whether the LEDs are disabled.
    /// </summary>
    public bool DisableLEDs { get; init; }
    /// <summary>
    /// Gets the LCD screen idle timeout (?).
    /// </summary>
    public ushort LCDScreenIdleTimeout { get; init; }
    /// <summary>
    /// Gets the lead acid battery calibration upper limit (?).
    /// </summary>
    public decimal LeadAcidBatteryCalibrationUpperLimit { get; init; }
    /// <summary>
    /// Gets the lead acid battery calibration lower limit (?).
    /// </summary>
    public decimal LeadAcidbatteryCalibrationLowerLimit { get; init; }
    /// <summary>
    /// Gets the inverter operating mode (?).
    /// </summary>
    public ushort InverterOperatingMode { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg353 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg354 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg355 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg356 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg357 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg358 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg359 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg360 { get; init; }
}
