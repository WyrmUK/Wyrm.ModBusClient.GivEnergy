using Wyrm.ModBusClient.GivEnergy.Constants;

namespace Wyrm.ModBusClient.GivEnergy.Responses;

/// <summary>
/// Properties from the first input data set from a GivEnergy inverter.
/// </summary>
public record InverterProperties1 : IResponseData
{
    /// <summary>
    /// Gets the device type code.
    /// </summary>
    public string DeviceTypeCode { get; init; } = string.Empty;
    /// <summary>
    /// Gets the model of the inverter.
    /// </summary>
    public InverterModel Model { get; init; } = InverterModel.Unknown;
    /// <summary>
    /// Gets the module code.
    /// </summary>
    public string Module { get; init; } = string.Empty;
    /// <summary>
    /// Gets the number of MPPT.
    /// </summary>
    public int NumberOfMPPT { get; init; }
    /// <summary>
    /// Gets the number of Phases.
    /// </summary>
    public int NumberOfPhases { get; init; }
    /// <summary>
    /// Unused.
    /// </summary>
    public ushort Reg005 { get; init; }
    /// <summary>
    /// Unused.
    /// </summary>
    public ushort Reg006 { get; init; }
    /// <summary>
    /// Unused.
    /// </summary>
    public ushort Reg007 { get; init; }
    /// <summary>
    /// Gets whether the ammeter is enabled.
    /// </summary>
    public bool EnableAmmeter { get; init; }
    /// <summary>
    /// Gets what was the first battery serial number but is now not used.
    /// </summary>
    public string UnusedSerialNumber { get; init; } = string.Empty;
    /// <summary>
    /// Gets the serial number.
    /// </summary>
    public string SerialNumber { get; init; } = string.Empty;
    /// <summary>
    /// Gets the firmware version of the first battery's BMS.
    /// </summary>
    public ushort FirstBatteryBMSFirmwareVersion { get; init; }
    /// <summary>
    /// Gets the firmware version of the DSP.
    /// </summary>
    public ushort DSPFirmwareVersion { get; init; }
    /// <summary>
    /// Gets whether the charge target is enabled.
    /// </summary>
    public bool EnableChargeTarget { get; init; }
    /// <summary>
    /// Gets the firmware version of the ARM.
    /// </summary>
    public ushort ARMFirmwareVersion { get; init; }
    /// <summary>
    /// Gets the firmware version string.
    /// </summary>
    public string FirmwareVersion => $"D0.{DSPFirmwareVersion}-A0.{ARMFirmwareVersion}";
    /// <summary>
    /// Gets the USB device type inserted.
    /// </summary>
    public UsbDeviceType UsbDeviceType { get; init; }
    /// <summary>
    /// Gets whether the ARM Chip is selected.
    /// </summary>
    public bool SelectARMChip { get; init; }
    /// <summary>
    /// Gets the variable address.
    /// </summary>
    public ushort VariableAddress { get; init; }
    /// <summary>
    /// Gets the variable value.
    /// </summary>
    public ushort VariableValue { get; init; }
    /// <summary>
    /// Gets the Grid Port maximum power output (W).
    /// </summary>
    public ushort GridPortMaximumPowerOutput { get; init; }
    /// <summary>
    /// Gets the battery power mode.
    /// </summary>
    public BatteryPowerMode BatteryPowerMode { get; init; }
    /// <summary>
    /// Gets whether 60Hz frequency mode is enabled.
    /// </summary>
    public bool Enable60HzFrequencyMode { get; init; }
    /// <summary>
    /// Gets the battery calibration stage.
    /// </summary>
    public BatteryCalibrationStage BatteryCalibrationStage { get; init; }
    /// <summary>
    /// Gets the MODBUS Address.
    /// </summary>
    public ushort ModbusAddress { get; init; }
    /// <summary>
    /// Gets the second charge slot.
    /// Null is no slot.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? ChargeSlot2 { get; init; }
    /// <summary>
    /// Gets the User Code.
    /// </summary>
    public ushort UserCode { get; init; }
    /// <summary>
    /// Gets the MODBUS version.
    /// </summary>
    public string ModbusVersion { get; init; } = string.Empty;
    /// <summary>
    /// Gets the system time.
    /// </summary>
    public DateTime SystemTime { get; init; }
    /// <summary>
    /// Gets whether the DRM RJ45 port is enabled.
    /// </summary>
    public bool EnableDRMRJ45Port { get; init; }
    /// <summary>
    /// Gets whether the Reversed CT Clamp is enabled.
    /// </summary>
    public bool EnableReversedCTClamp { get; init; }
    /// <summary>
    /// Gets the charging SOC (%).
    /// </summary>
    public int ChargeState { get; init; }
    /// <summary>
    /// Cats the discharging SOC (%).
    /// </summary>
    public int DischargeState { get; init; }
    /// <summary>
    /// Gets the second discharge slot.
    /// Null is no slot.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? DischargeSlot2 { get; init; }
    /// <summary>
    /// Gets the firmware version of the BMS.
    /// </summary>
    public ushort BMSFirmwareVersion { get; init; }
    /// <summary>
    /// Gets the meter type.
    /// </summary>
    public MeterType MeterType { get; init; }
    /// <summary>
    /// Gets whether the reversed 115 meter is enabled.
    /// </summary>
    public bool EnableReversed115Meter { get; init; }
    /// <summary>
    /// Gets whether the reversed 418 meter is enabled.
    /// </summary>
    public bool EnableReversed418Meter { get; init; }
    /// <summary>
    /// Gets the active power rate (?).
    /// </summary>
    public ushort ActivePowerRate { get; init; }
    /// <summary>
    /// Gets the reactive power rate (?).
    /// </summary>
    public ushort ReactivePowerRate { get; init; }
    /// <summary>
    /// Gets the power factor.
    /// </summary>
    public decimal PowerFactor { get; init; }
    /// <summary>
    /// Gets whether the inverter auto restart is enabled.
    /// </summary>
    public bool EnableInverterAutoRestart { get; init; }
    /// <summary>
    /// Gets whether the inverter is enabled.
    /// </summary>
    public bool EnableInverter { get; init; }
    /// <summary>
    /// Gets the battery type.
    /// </summary>
    public BatteryType BatteryType { get; init; }
    /// <summary>
    /// Gets the battery capacity (Ah).
    /// </summary>
    public ushort BatteryCapacity { get; init; }
    /// <summary>
    /// Gets the first discharge slot.
    /// Null is no slot.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? DischargeSlot1 { get; init; }
    /// <summary>
    /// Gets whether auto judge battery type is enabled.
    /// </summary>
    public bool EnableAutoJudgeBatteryType { get; init; }
    /// <summary>
    /// Gets whether discharge is enabled.
    /// </summary>
    public bool EnableDischarge { get; init; }
}
