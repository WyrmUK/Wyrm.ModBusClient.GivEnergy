namespace Wyrm.ModBusClient.GivEnergy.Responses;

/// <summary>
/// Properties from the second data set from a GivEnergy Battery.
/// </summary>
public record BatteryData2 : IResponseData
{
    /// <summary>
    /// Gets the battery device number.
    /// </summary>
    public int Device { get; init; }
    /// <summary>
    /// Gets the individual cell voltages (V).
    /// </summary>
    public decimal[] CellVoltages { get; init; } = [];
    /// <summary>
    /// Gets the temperature for blocks of four cells (C).
    /// </summary>
    public decimal[] CellsTemperature { get; init; } = [];
    /// <summary>
    /// Gets the sum of all cell voltages (V).
    /// </summary>
    public decimal CellVoltageSum { get; init; }
    /// <summary>
    /// Gets the temperature of the BMS Mosfet (V).
    /// </summary>
    public decimal BMSMosfetTemperature { get; init; }
    /// <summary>
    /// Gets the output voltage of the battery (V).
    /// </summary>
    public decimal OutVoltage { get; init; }
    /// <summary>
    /// Gets the calibrated capacity (?).
    /// </summary>
    public decimal CalibratedCapacity { get; init; }
    /// <summary>
    /// Gets the design capacity (?).
    /// </summary>
    public decimal DesignCapacity { get; set; }
    /// <summary>
    /// Gets the remaining capacity (?).
    /// </summary>
    public decimal RemainingCapacity { get; init; }
    /// <summary>
    /// Gets the first status byte.
    /// </summary>
    public byte Status1 { get; init; }
    /// <summary>
    /// Gets the second status byte.
    /// </summary>
    public byte Status2 { get; init; }
    /// <summary>
    /// Gets the third status byte.
    /// </summary>
    public byte Status3 { get; init; }
    /// <summary>
    /// Gets the fourth status byte.
    /// </summary>
    public byte Status4 { get; init; }
    /// <summary>
    /// Gets the fifth status byte.
    /// </summary>
    public byte Status5 { get; init; }
    /// <summary>
    /// Gets the sixth status byte.
    /// </summary>
    public byte Status6 { get; init; }
    /// <summary>
    /// Gets the seventh status byte.
    /// </summary>
    public byte Status7 { get; init; }
    /// <summary>
    /// Gets the first warning byte.
    /// </summary>
    public byte Warning1 { get; init; }
    /// <summary>
    /// Gets the second warning byte.
    /// </summary>
    public byte Warning2 { get; init; }
    /// <summary>
    /// Gets the battery current with -ve being charging (A).
    /// </summary>
    public decimal Current { get; init; }
    /// <summary>
    /// Gets the number of cycles for the battery.
    /// </summary>
    public ushort NumberOfCycles { get; init; }
    /// <summary>
    /// Gets the number of cells in the battery.
    /// </summary>
    public ushort NumberofCells { get; init; }
    /// <summary>
    /// Gets the BMS firmware version.
    /// </summary>
    public ushort BMSFirmwareVersion { get; init; }
    /// <summary>
    /// Unused.
    /// </summary>
    public ushort Reg160 { get; init; }
    /// <summary>
    /// Gets the percentage charge (%).
    /// </summary>
    public ushort StateOfCharge { get; init; }
    /// <summary>
    /// Gets the second design capacity (?).
    /// </summary>
    public decimal DesignCapacity2 { get; init; }
    /// <summary>
    /// Gets the maximum temperature (C).
    /// </summary>
    public decimal MaximumTemperature { get; init; }
    /// <summary>
    /// Gets the minimum temperature (C).
    /// </summary>
    public decimal MinimumTemperature { get; init; }
    /// <summary>
    /// Gets the discharge total energy (kWh).
    /// </summary>
    public decimal DischargeTotalEnergy { get; init; }
    /// <summary>
    /// Gets the charge total energy (kWh).
    /// </summary>
    public decimal ChargeTotalEnergy { get; init; }
    /// <summary>
    /// Gets the force discharge flag.
    /// </summary>
    public ushort ForceDischargeFlag { get; init; }
    /// <summary>
    /// Unused.
    /// </summary>
    public ushort Reg169 { get; init; }
    /// <summary>
    /// Unused.
    /// </summary>
    public ushort Reg170 { get; init; }
    /// <summary>
    /// Gets the serial number.
    /// </summary>
    public string SerialNumber { get; init; } = string.Empty;
    /// <summary>
    /// Gets the USB device identifier.
    /// </summary>
    public ushort UsbDevice { get; init; }
    /// <summary>
    /// Unused.
    /// </summary>
    public ushort Reg177 { get; init; }
    /// <summary>
    /// Unused.
    /// </summary>
    public ushort Reg178 { get; init; }
    /// <summary>
    /// Unused.
    /// </summary>
    public ushort Reg179 { get; init; }
    /// <summary>
    /// Unused.
    /// </summary>
    public ushort Reg180 { get; init; }
}
