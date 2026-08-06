using Wyrm.ModBusClient.GivEnergy.Constants;

namespace Wyrm.ModBusClient.GivEnergy.Responses;

/// <summary>
/// Properties from the fourth input data set from a GivEnergy inverter.
/// </summary>
public record InverterProperties4 : IResponseData
{
    /// <summary>
    /// Gets registers 181 to 199.
    /// </summary>
    public ushort[] Reg181To199 { get; init; } = [];
    /// <summary>
    /// Gets whether the inverter parallel mode is enabled.
    /// </summary>
    public bool EnableInverterParallelMode { get; init; }
    /// <summary>
    /// Starts the BMS flash update.
    /// </summary>
    public bool CommandBMDFlashUpdate { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg202 { get; init; }
    /// <summary>
    /// Unknown.
    /// </summary>
    public ushort Reg203 { get; init; }
    /// <summary>
    /// Gets the inverter errors.
    /// </summary>
    public uint InverterErrors { get; init; }
    /// <summary>
    /// Gets the inverter fault codes (decoded InverterErrors).
    /// </summary>
    public ICollection<InverterFaultCode> InverterFaultCodes { get; init; } = [];
    /// <summary>
    /// Gets the registers 226 to 240.
    /// </summary>
    public ushort[] Reg206To240 { get; init; } = [];
}
