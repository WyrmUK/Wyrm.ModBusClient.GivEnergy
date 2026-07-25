namespace Wyrm.ModBusClient.GivEnergy.Responses;

/// <summary>
/// Properties from the second data set from a GivEnergy Low Voltage BCU.
/// </summary>
public record LowVoltageBCUData2 : IResponseData
{
    /// <summary>
    /// Gets the BMS Status value 1.
    /// </summary>
    public ushort BMSStatus1 { get; init; }
    /// <summary>
    /// Gets the BMS Status value 2.
    /// </summary>
    public ushort BMSStatus2 { get; init; }
    /// <summary>
    /// Gets the request charge current (A).
    /// </summary>
    public ushort RequestChargeCurrent { get; init; }
    /// <summary>
    /// Gets the request discharge current (A).
    /// </summary>
    public ushort RequestDischargeCurrent { get; init; }
}
