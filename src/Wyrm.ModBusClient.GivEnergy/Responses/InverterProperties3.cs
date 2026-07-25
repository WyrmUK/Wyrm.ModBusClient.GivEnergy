using Wyrm.ModBusClient.GivEnergy.Constants;

namespace Wyrm.ModBusClient.GivEnergy.Responses;

/// <summary>
/// Properties from the third input data set from a GivEnergy inverter.
/// </summary>
public record InverterProperties3 : IResponseData
{
    /// <summary>
    /// Gets the discharge SOC stop 1 value (?).
    /// </summary>
    public ushort DischargeStateOfChargeStop1 { get; init; }
    /// <summary>
    /// Gets whether the local command test is enabled.
    /// </summary>
    public bool EnableLocalCommandTest { get; init; }
    /// <summary>
    /// Gets the power function model.
    /// </summary>
    public PowerFactorFunction PowerFactorFunction { get; init; }
    /// <summary>
    /// Gets the frequenct load limit rate (?).
    /// </summary>
    public ushort FrequencyLoadLimitRate { get; init; }
    /// <summary>
    /// Gets whether the low voltage fault ride through is enabled.
    /// </summary>
    public bool EnableLowVoltageFaultRideThrough { get; init; }
    /// <summary>
    /// Gets whether the frequency derating is enabled.
    /// </summary>
    public bool EnableFrequencyDerating { get; init; }
    /// <summary>
    /// Gets whether above 6kW system is enabled.
    /// </summary>
    public bool EnableAbove6kWSystem { get; init; }
    /// <summary>
    /// Gets whether the system auto test is started.
    /// </summary>
    public bool StartSystemAutoTest { get; init; }
    /// <summary>
    /// Gets whether SPI is enabled.
    /// </summary>
    public bool EnableSPI { get; init; }
    /// <summary>
    /// Gets the power factor command memory state (?).
    /// </summary>
    public ushort PowerFactorCommandMemoryState { get; init; }
    /// <summary>
    /// Gets the power factor points load percentage (%) and power factor (?) for points 1 to 4.
    /// </summary>
    public (ushort LoadPercent, ushort PowerFactor)[] PowerFactorPoints { get; init; } = [];
    /// <summary>
    /// Gets the CEI 021 V1 S Q (?).
    /// </summary>
    public decimal CEI021V1SQuotient { get; init; }
    /// <summary>
    /// Gets the CEI 021 V2 S Q (?).
    /// </summary>
    public decimal CEI021V2SQuotient { get; init; }
    /// <summary>
    /// Gets the CEI 021 V1 L Q (?).
    /// </summary>
    public decimal CEI021V1LQuotient { get; init; }
    /// <summary>
    /// Gets the CEI 021 V2 L Q (?).
    /// </summary>
    public decimal CEI021V2LQuotient { get; init; }
    /// <summary>
    /// Gets the CEI 021 lock in active power (W).
    /// </summary>
    public ushort CEI021LockInActivePower { get; init; }
    /// <summary>
    /// Gets the CEI 021 lock out active power (W).
    /// </summary>
    public ushort CEI021LockOutActivePower { get; init; }
    /// <summary>
    /// Gets the CEI 021 lock in grid voltage (V).
    /// </summary>
    public decimal CEI021LockInGridVoltage { get; init; }
    /// <summary>
    /// Gets the CEI 021 lock out grid voltage (V).
    /// </summary>
    public decimal CEI021LockOutGridVoltage { get; init; }
    /// <summary>
    /// Gets the LV FRT reactive rate.
    /// </summary>
    public ushort LVFRTReactiveRate { get; init; }
    /// <summary>
    /// Gets the LVFRT low fault value (?) and time (?) for faults 1 to 4.
    /// </summary>
    public (ushort Value, ushort Time)[] LVFRTLowFaults { get; init; } = [];
    /// <summary>
    /// Gets the LVFRT high fault value (?) and time (?) for fault 1.
    /// </summary>
    public (ushort Value, ushort Time)[] LVFRTHighFaults { get; init; } = [];
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg158 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg159 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg160 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg161 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg162 { get; init; }
    /// <summary>
    /// Momentary write trigger only.
    /// </summary>
    public ushort ResetUserInformation { get; init; }
    /// <summary>
    /// Written to only (writing the wrong value can cause the inverter to brick itself).
    /// </summary>
    public ushort InverterReboot { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg165 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg166 { get; init; }
    /// <summary>
    /// Gets whether the real time control is enabled.
    /// </summary>
    public bool EnableRealTimeControl { get; init; }
    /// <summary>
    /// Gets the three phase balance mode (?);
    /// </summary>
    public ushort ThreePhaseBalanceMode { get; init; }
    /// <summary>
    /// Gets the three phase ABC (?);
    /// </summary>
    public ushort ThreePhaseABC { get; init; }
    /// <summary>
    /// Gets the three phase balance 1 (?).
    /// </summary>
    public ushort ThreePhaseBalance1 { get; init; }
    /// <summary>
    /// Gets the three phase balance 2 (?).
    /// </summary>
    public ushort ThreePhaseBalance2 { get; init; }
    /// <summary>
    /// Gets the three phase balance 3 (?).
    /// </summary>
    public ushort ThreePhaseBalance3 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg173 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg174 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg175 { get; init; }
    /// <summary>
    /// Gets whether the battery is enabled on PV or Grid.
    /// </summary>
    public bool EnableBatteryOnPVOrGrid { get; init; }
    /// <summary>
    /// Sets debug mode.
    /// </summary>
    public ushort DebugInverter { get; init; }
    /// <summary>
    /// Gets whether UPS mode is enabled.
    /// </summary>
    public bool EnableUPSMode { get; init; }
    /// <summary>
    /// Gets whether the G100 limit switch is enabled.
    /// </summary>
    public bool EnableG100LimitSwitch { get; init; }
    /// <summary>
    /// Gets whether the battery cable impedance alarm is enabled.
    /// </summary>
    public bool EnableBatteryCableImpedanceAlarm { get; init; }
}
