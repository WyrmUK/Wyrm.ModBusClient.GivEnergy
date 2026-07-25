namespace Wyrm.ModBusClient.GivEnergy.Constants;

/// <summary>
/// The various Status values.
/// </summary>
public enum GivEnergyStatus : ushort
{
    /// <summary>
    /// Waiting.
    /// </summary>
    Waiting,
    /// <summary>
    /// Normal operation.
    /// </summary>
    Normal,
    /// <summary>
    /// Warning.
    /// </summary>
    Warning,
    /// <summary>
    /// Fault.
    /// </summary>
    Fault,
    /// <summary>
    /// Flashing update.
    /// </summary>
    FlashingUpdate
}
