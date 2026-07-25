using Wyrm.ModBusClient.GivEnergy.Responses.Models;

namespace Wyrm.ModBusClient.GivEnergy.Responses;

/// <summary>
/// Properties from the second data set from a GivEnergy meter.
/// </summary>
public record MeterData2 : IResponseData
{
    /// <summary>
    /// Gets the meter device number.
    /// </summary>
    public int Device { get; init; }
    /// <summary>
    /// Gets the phase 1 data.
    /// </summary>
    public MeterPhaseData Phase1 { get; init; } = new();
    /// <summary>
    /// Gets the phase 2 data.
    /// All zero if single phase.
    /// </summary>
    public MeterPhaseData Phase2 { get; init; } = new();
    /// <summary>
    /// Gets the phase 3 data.
    /// All zero if single phase.
    /// </summary>
    public MeterPhaseData Phase3 { get; init; } = new();
    /// <summary>
    /// Gets the neutral line current (A).
    /// </summary>
    public decimal LineCurrent { get; init; }
    /// <summary>
    /// Gets the total current (A).
    /// </summary>
    public decimal TotalCurrent { get; init; }
    /// <summary>
    /// Gets the total active power (W).
    /// </summary>
    public decimal ActiveTotalPower { get; init; }
    /// <summary>
    /// Gets the total reactive power (W).
    /// </summary>
    public decimal ReactiveTotalPower { get; init; }
    /// <summary>
    /// Gets the total apparent (magnitude) power (W).
    /// </summary>
    public decimal ApparentTotalPower { get; init; }
    /// <summary>
    /// Gets the total power factor.
    /// </summary>
    public decimal TotalPowerFactor { get; init; }
    /// <summary>
    /// Gets the frequency (Hz).
    /// </summary>
    public decimal Frequency { get; init; }
    /// <summary>
    /// Gets the active import energy (kWh).
    /// </summary>
    public decimal ActiveImportEnergy { get; init; }
    /// <summary>
    /// Gets the reactive import energy (kWh).
    /// </summary>
    public decimal ReactiveImportEnergy { get; init; }
    /// <summary>
    /// Gets the active export energy (kWh).
    /// </summary>
    public decimal ActiveExportEnergy { get; init; }
    /// <summary>
    /// Gets the reactive export energy (kWh).
    /// </summary>
    public decimal ReactiveExportEnergy { get; init; }
}
