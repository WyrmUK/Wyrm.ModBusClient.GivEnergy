namespace Wyrm.ModBusClient.GivEnergy.Responses;

/// <summary>
/// Undecoded register values.
/// </summary>
public record RegisterData : IResponseData
{
    /// <summary>
    /// Gets the device address/unit identifier.
    /// </summary>
    public byte UnitIdentifier { get; init; }
    /// <summary>
    /// Gets the function number (3 = holding registers, 4 = input registers).
    /// </summary>
    public byte FunctionNumber { get; init; }
    /// <summary>
    /// Gets the start register number.
    /// </summary>
    public int StartAddress { get; init; }
    /// <summary>
    /// Gets the register values.
    /// </summary>
    public IReadOnlyList<ushort> RegisterValues { get; init; } = [];
}
