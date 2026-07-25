namespace Wyrm.ModBusClient.GivEnergy.Responses;

/// <summary>
/// Properties from the tenth input data set from a GivEnergy inverter.
/// Smart Load scheduling.
/// </summary>
public record InverterProperties10 : IResponseData
{
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort[] Reg541To554 { get; init; } = [];
    /// <summary>
    /// Gets the smart load time slot 1.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? SmartLoadSlot1 { get; init; }
    /// <summary>
    /// Gets the smart load time slot 2.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? SmartLoadSlot2 { get; init; }
    /// <summary>
    /// Gets the smart load time slot 3.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? SmartLoadSlot3 { get; init; }
    /// <summary>
    /// Gets the smart load time slot 4.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? SmartLoadSlot4 { get; init; }
    /// <summary>
    /// Gets the smart load time slot 5.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? SmartLoadSlot5 { get; init; }
    /// <summary>
    /// Gets the smart load time slot 6.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? SmartLoadSlot6 { get; init; }
    /// <summary>
    /// Gets the smart load time slot 7.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? SmartLoadSlot7 { get; init; }
    /// <summary>
    /// Gets the smart load time slot 8.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? SmartLoadSlot8 { get; init; }
    /// <summary>
    /// Gets the smart load time slot 9.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? SmartLoadSlot9 { get; init; }
    /// <summary>
    /// Gets the smart load time slot 10.
    /// </summary>
    public (TimeOnly Start, TimeOnly End)? SmartLoadSlot10 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort[] Reg575To600 { get; init; } = [];
}
