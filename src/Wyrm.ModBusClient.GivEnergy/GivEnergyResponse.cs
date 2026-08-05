using Wyrm.ModBusClient.GivEnergy.Responses;
using Wyrm.ModBusClient.GivEnergy.Responses.Constants;

namespace Wyrm.ModBusClient.GivEnergy;

/// <summary>
/// The GivEnergy response received from the inverter.
/// </summary>
public record GivEnergyResponse
{
    /// <summary>
    /// Gets the serial number of the inverter.
    /// </summary>
    public required string SerialNumber { get; init; }
    /// <summary>
    /// Gets the serial number of the Wifi Adapter.
    /// </summary>
    public required string WifiAdapter { get; init; }
    /// <summary>
    /// Gets the device number of the set of devices the data could be for (1 is the first device).
    /// Usually 1, except for Meter and Batttery data.
    /// </summary>
    public byte DeviceNumber { get; init; } = 1;
    /// <summary>
    /// Gets the response data type returned.
    /// Use this to cast the ResponseData to the correct type.
    /// </summary>
    public required ResponseDataType ResponseDataType { get; init; }
    /// <summary>
    /// Gets the response data item.
    /// This could be any class that derives from <see cref="IResponseData"/>.
    /// </summary>
    public required IResponseData ResponseData { get; init; }
}
