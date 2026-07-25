namespace Wyrm.ModBusClient.GivEnergy.Responses;

/// <summary>
/// Properties from the second input data set from a GivEnergy inverter.
/// </summary>
public record InverterProperties2 : IResponseData
{
    /// <summary>
    /// Gets the PV start voltage (V).
    /// </summary>
    public decimal PVStartVoltage { get; init; }
    /// <summary>
    /// Gets the start countdown timer value (?).
    /// </summary>
    public ushort StartCountdownTimer { get; init; }
    /// <summary>
    /// Gets the restart delay time value (?).
    /// </summary>
    public ushort RestartDelayTime { get; init; }
    /// <summary>
    /// Gets the AC low limit trip voltage (V).
    /// </summary>
    public decimal ACLowLimitTripVoltage { get; init; }
    /// <summary>
    /// Gets the AC high limit trip voltage (V).
    /// </summary>
    public decimal ACHighLimitTripVoltage { get; init; }
    /// <summary>
    /// Gets the AC low limit trip frequency (Hz).
    /// </summary>
    public decimal ACLowLimitTripFrequency { get; init; }
    /// <summary>
    /// Gets the AC high limit trip frequency (Hz).
    /// </summary>
    public decimal ACHighLimitTripFrequency { get; init; }
    /// <summary>
    /// Gets the AC low voltage trip time (?).
    /// </summary>
    public decimal ACLowVoltageTripTime { get; init; }
    /// <summary>
    /// Gets the AC high voltage trip time (?).
    /// </summary>
    public decimal ACHighVoltageTripTime { get; init; }
    /// <summary>
    /// Gets the AC low frequency trip time (?).
    /// </summary>
    public decimal ACLowFrequencyTripTime { get; init; }
    /// <summary>
    /// Gets the AC high frequency trip time (?).
    /// </summary>
    public decimal ACHighFrequencyTripTime { get; init; }
    /// <summary>
    /// Gets the AC low limit reconnect voltage (V).
    /// </summary>
    public decimal ACLowLimitReconnectVoltage { get; init; }
    /// <summary>
    /// Gets the AC high limit reconnect voltage (V).
    /// </summary>
    public decimal ACHighLimitReconnectVoltage { get; init; }
    /// <summary>
    /// Gets the AC low limit reconnect frequency (Hz).
    /// </summary>
    public decimal ACLowLimitReconnectFrequency { get; init; }
    /// <summary>
    /// Gets the AC high limit reconnect frequency (Hz).
    /// </summary>
    public decimal ACHighLimitReconnectFrequency { get; init; }
    /// <summary>
    /// Gets the AC low voltage reconnect time (?).
    /// </summary>
    public decimal ACLowVoltageReconnectTime { get; init; }
    /// <summary>
    /// Gets the AC high voltage reconnect time (?).
    /// </summary>
    public decimal ACHighVoltageReconnectTime { get; init; }
    /// <summary>
    /// Gets the AC low frequency reconnect time (Hz).
    /// </summary>
    public decimal ACLowFrequencyReconnectTime { get; init; }
    /// <summary>
    /// Gets the AC high frequency reconnect time (Hz).
    /// </summary>
    public decimal ACHighFrequencyReconnectTime { get; init; }
    /// <summary>
    /// Gets the low limit grid voltage (V).
    /// </summary>
    public decimal ACLowLimitGridVoltage { get; init; }
    /// <summary>
    /// Gets the high limit grid voltage (V).
    /// </summary>
    public decimal ACHighLimitGridVoltage { get; init; }
    /// <summary>
    /// Gets the low limit grid frequency (Hz).
    /// </summary>
    public decimal ACLowLimitGridFrequency { get; init; }
    /// <summary>
    /// Gets the high limit grid frequency (Hz).
    /// </summary>
    public decimal ACHighLimitGridFrequency { get; init; }
    /// <summary>
    /// Gets the AC 10 minute protect voltage (V).
    /// </summary>
    public decimal AC10MinuteProtectVoltage { get; init; }
    /// <summary>
    /// Gets the ISO protection 1 value (?).
    /// </summary>
    public ushort ISOProtection1 { get; init; }
    /// <summary>
    /// Gets the ISO protection 2 value (?).
    /// </summary>
    public ushort ISOProtection2 { get; init; }
    /// <summary>
    /// Gets the GFCI protection value 1 (?),
    /// </summary>
    public ushort GFCIProtectionValue1 { get; init; }
    /// <summary>
    /// Gets the GFCI protection time 1 (?),
    /// </summary>
    public ushort GFCIProtectionTime1 { get; init; }
    /// <summary>
    /// Gets the GFCI protection value 2 (?),
    /// </summary>
    public ushort GFCIProtectionValue2 { get; init; }
    /// <summary>
    /// Gets the GFCI protection time 2 (?),
    /// </summary>
    public ushort GFCIProtectionTime2 { get; init; }
    /// <summary>
    /// Gets the DCI protection value 1 (?),
    /// </summary>
    public ushort DCIProtectionValue1 { get; init; }
    /// <summary>
    /// Gets the DCI protection time 1 (?),
    /// </summary>
    public ushort DCIProtectionTime1 { get; init; }
    /// <summary>
    /// Gets the DCI protection value 2 (?),
    /// </summary>
    public ushort DCIProtectionValue2 { get; init; }
    /// <summary>
    /// Gets the DCI protection time 2 (?),
    /// </summary>
    public ushort DCIProtectionTime2 { get; init; }
    /// <summary>
    /// Gets the first charge slot.
    /// Null is no slot.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? ChargeSlot1 { get; init; }
    /// <summary>
    /// Gets whether charging is enabled..
    /// </summary>
    public bool EnableCharge { get; init; }
    /// <summary>
    /// Gets the battery low voltage protection limit (?).
    /// </summary>
    public decimal BatteryLowVoltageProtectionLimit { get; init; }
    /// <summary>
    /// Gets the battery high voltage protection limit (?).
    /// </summary>
    public decimal BatteryHighVoltageProtectionLimit { get; init; }
    /// <summary>
    /// Gets the string 1 voltage adjustment value (?).
    /// </summary>
    public ushort String1VoltageAdjustment { get; init; }
    /// <summary>
    /// Gets the string 2 voltage adjustment value (?).
    /// </summary>
    public ushort String2VoltageAdjustment { get; init; }
    /// <summary>
    /// Gets the grid import limit (?).
    /// </summary>
    public ushort GridImportLimit { get; init; }
    /// <summary>
    /// Gets whether the grid import limit is enabled.
    /// </summary>
    public bool GridImportLimitEnabled { get; init; }
    /// <summary>
    /// Gets whether LORA is enabled.
    /// </summary>
    public bool EnableLORA { get; init; }
    /// <summary>
    /// Gets whether battery self heating is enabled.
    /// </summary>
    public bool EnableBatterySelfHeating { get; init; }
    /// <summary>
    /// Gets the battery voltage adjust (?).
    /// </summary>
    public decimal BatteryVoltageAdjust { get; init; }
    /// <summary>
    /// Gets the string 1 power adjustment value (?).
    /// </summary>
    public ushort String1PowerAdjustment { get; init; }
    /// <summary>
    /// Gets the string 2 power adjustment value (?).
    /// </summary>
    public ushort String2PowerAdjustment { get; init; }
    /// <summary>
    /// Gets the battery low force charge time value (?).
    /// </summary>
    public ushort BatteryLowForceChargeTime { get; init; }
    /// <summary>
    /// Gets whether BMS Read is enabled.
    /// </summary>
    public bool EnableBMSRead { get; init; }
    /// <summary>
    /// Gets the battery SOC reserve value (?).
    /// </summary>
    public ushort BatteryStateOfChargeReserve { get; init; }
    /// <summary>
    /// Gets the battery charge limit (?).
    /// </summary>
    public ushort BatteryChargeLimit { get; init; }
    /// <summary>
    /// Gets the battery discharge limit (?).
    /// </summary>
    public ushort BatteryDischargeLimit { get; init; }
    /// <summary>
    /// Gets whether the buzzer is enabled.
    /// </summary>
    public bool EnableBuzzer { get; init; }
    /// <summary>
    /// Gets the battery discharge minimum power reserve value (?).
    /// </summary>
    public ushort BatteryDischargeMinPowerReserve { get; init; }
    /// <summary>
    /// Gets the value of register 116 (IS_LAN).
    /// </summary>
    public ushort Reg116 { get; init; }
    /// <summary>
    /// Gets the charge target SOC (?).
    /// </summary>
    public ushort ChargeTargetStateOfCharge { get; init; }
    /// <summary>
    /// Gets the charge SOC stop 2 value (?).
    /// </summary>
    public ushort ChargeStateOfChargeStop2 { get; init; }
    /// <summary>
    /// Gets the discharge SOC stop 2 value (?).
    /// </summary>
    public ushort DischargeStateOfChargeStop2 { get; init; }
    /// <summary>
    /// Gets the charge SOC stop 1 value (?).
    /// </summary>
    public ushort ChargeStateOfChargeStop1 { get; init; }
}
