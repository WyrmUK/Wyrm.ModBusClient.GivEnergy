namespace Wyrm.ModBusClient.GivEnergy.Responses;

/// <summary>
/// Properties from the fifth data set from a GivEnergy inverter.
/// </summary>
public record InverterData5 : IResponseData
{
    /// <summary>
    /// Gets a possible voltage field (V).
    /// Possible Combined Generation Voltage (average).
    /// </summary>
    public decimal Voltage5 { get; init; }
    /// <summary>
    /// Gets a possible current field (A).
    /// Possible Combined Generation Current (average).
    /// </summary>
    public decimal Current5 { get; init; }
    /// <summary>
    /// Gets the combined power generation (W).
    /// </summary>
    public decimal CombinedGenerationPower { get; init; }
}
