namespace Wyrm.ModBusClient.GivEnergy;

/// <summary>
/// Represents data errors.
/// </summary>
public class GivEnergyClientException : Exception
{
    internal GivEnergyClientException(string? message, ReadOnlyMemory<byte> data, Exception? innerException = null) :
        base(message, innerException)
    {
        ErrorData = data;
    }

    internal GivEnergyClientException(string? message, Exception? innerException = null) :
        base(message, innerException)
    {
        ErrorData = new ReadOnlyMemory<byte>();
    }

    /// <summary>
    /// Gets the data causing the error.
    /// </summary>
    public ReadOnlyMemory<byte> ErrorData { get; }
}
