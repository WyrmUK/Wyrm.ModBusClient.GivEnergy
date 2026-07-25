namespace Wyrm.ModBusClient.GivEnergy.Constants;

/// <summary>
/// The Power Factor functions.
/// </summary>
public enum PowerFactorFunction : ushort
{
    /// <summary>
    /// PF 1.
    /// </summary>
    PF1,
    /// <summary>
    /// PF by set.
    /// </summary>
    PFBySet,
    /// <summary>
    /// Default PF line.
    /// </summary>
    DefaultPFLine,
    /// <summary>
    /// User PF line.
    /// </summary>
    UserPFLine,
    /// <summary>
    /// Under excited inductive/reactive power.
    /// </summary>
    UnderExcitedInductiveReactivePower,
    /// <summary>
    /// Over excited inductive/reactive power.
    /// </summary>
    OverExcitedInductiveReactivePower,
    /// <summary>
    /// QV model.
    /// </summary>
    QVModel
}
