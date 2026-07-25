namespace Wyrm.ModBusClient.GivEnergy.Responses.Models;

/// <summary>
/// The meter data for a specific phase.
/// </summary>
public record MeterPhaseData
{
    /// <summary>
    /// Gets the voltage for the phase (V).
    /// </summary>
    public decimal Voltage { get; init; }
    /// <summary>
    /// Gets the current for the phase (A).
    /// </summary>
    public decimal Current { get; init; }
    /// <summary>
    /// Gets the active power for the phase (W).
    /// </summary>
    public decimal ActivePower { get; init; }
    /// <summary>
    /// Gets the reactive power for the phase (W).
    /// </summary>
    public decimal ReactivePower { get; init; }
    /// <summary>
    /// Gets the apparent power (magnitude) for the phase (W).
    /// </summary>
    public decimal ApparentPower { get; init; }
    /// <summary>
    /// Gets the power factor for the phase.
    /// </summary>
    public decimal PowerFactor { get; init; }
}
