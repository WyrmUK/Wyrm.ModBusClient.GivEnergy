namespace Wyrm.ModBusClient.GivEnergy.Constants;

/// <summary>
/// The different inverter models.
/// </summary>
public enum InverterModel : ushort
{
    /// <summary>
    /// Unknown.
    /// </summary>
    Unknown = 0x0000,
    /// <summary>
    /// Hybrid family.
    /// </summary>
    Hybrid = 0x2000,
    /// <summary>
    /// Hybrid Gen 1.
    /// </summary>
    HybridGen1 = 0x2001,
    /// <summary>
    /// Hybrid Gen 2.
    /// </summary>
    HybridGen2 = 0x2002,
    /// <summary>
    /// Hybrid Gen 3.
    /// </summary>
    HybridGen3 = 0x2003,
    /// <summary>
    /// Polar (Hybrid).
    /// </summary>
    Polar = 0x2100,
    /// <summary>
    /// AC family.
    /// </summary>
    AC = 0x3000,
    /// <summary>
    /// Hybrid 3 Phase family.
    /// </summary>
    Hybrid3PH = 0x4000,
    /// <summary>
    /// AIO Commercial.
    /// </summary>
    AIOCommercial = 0x4100,
    /// <summary>
    /// EMS family.
    /// </summary>
    EMS = 0x5000,
    /// <summary>
    /// EMS Commercial.
    /// </summary>
    EMSCommercial = 0x5100,
    /// <summary>
    /// AC 3 PHase family.
    /// </summary>
    AC3PH = 0x6000,
    /// <summary>
    /// Gateway family.
    /// </summary>
    Gateway = 0x7000,
    /// <summary>
    /// All In One family.
    /// </summary>
    AllInOne = 0x8000,
    /// <summary>
    /// Hybrid HV Gen 3.
    /// </summary>
    HybridHVGen3 = 0x8100,
    /// <summary>
    /// All In One Hybrid.
    /// </summary>
    AllInOneHybrid = 0x8200,
    /// <summary>
    /// Hybrid Gen 4.
    /// </summary>
    HybridGen4 = 0x8300
}
