namespace Wyrm.ModBusClient.GivEnergy.Responses;

/// <summary>
/// Properties from the ninth input data set from a GivEnergy inverter.
/// HV cabinet topology.
/// </summary>
public record InverterProperties9 : IResponseData
{
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort[] Reg481To499 { get; init; } = [];
    /// <summary>
    /// Gets the HV cabinet count.
    /// </summary>
    public ushort HVCabinetCount { get; init; }
    /// <summary>
    /// Gets the HV racks per cabinet.
    /// </summary>
    public ushort HVRacksPerCabinet { get; init; }
    /// <summary>
    /// Gets the HV batteries per rack.
    /// </summary>
    public ushort HVBatteriesPerRack { get; init; }
    /// <summary>
    /// Gets the HV cells per battery.
    /// </summary>
    public ushort HVCellsPerBattery { get; init; }
    /// <summary>
    /// Gets the HV total cells.
    /// </summary>
    public ushort HVTotalCells { get; init; }
    /// <summary>
    /// Gets the HV temperature sensors per battery.
    /// </summary>
    public ushort HVTemperatureSensorsPerBattery { get; init; }
    /// <summary>
    /// Gets the HV total temperature sensors.
    /// </summary>
    public ushort HVTotalTemperatureSensors { get; init; }
    /// <summary>
    /// Gets the HV max PCS power (W).
    /// </summary>
    public ushort HVMaxPCSPower { get; init; }
    /// <summary>
    /// Gets the HV max charge voltage (V).
    /// </summary>
    public decimal HVMaxChargeVoltage { get; init; }
    /// <summary>
    /// Gets the HV max discharge voltage (V).
    /// </summary>
    public decimal HVMinDischargeVoltage { get; init; }
    /// <summary>
    /// Gets the HV max charge current (A).
    /// </summary>
    public ushort HVMaxChargeCurrent { get; init; }
    /// <summary>
    /// Gets the HV parallel count.
    /// </summary>
    public ushort HVParallelCount { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort[] Reg512To540 { get; init; } = [];
}
