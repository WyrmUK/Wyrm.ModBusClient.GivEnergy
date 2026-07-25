namespace Wyrm.ModBusClient.GivEnergy.Constants;

/// <summary>
/// The various Charge Status values.
/// </summary>
public enum ChargeStatus : ushort
{
    /// <summary>
    /// Idle.
    /// </summary>
    Idle,
    /// <summary>
    /// Charging.
    /// </summary>
    Charging,
    /// <summary>
    /// Finishing.
    /// </summary>
    Finishing,
    /// <summary>
    /// Discharging.
    /// </summary>
    Discharging,
    /// <summary>
    /// Unknown.
    /// </summary>
    Unknown
}
