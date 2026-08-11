namespace Wyrm.ModBusClient.GivEnergy.Services;

internal interface IFramerService
{
    IList<byte> PduFramer(IList<byte> command);
    ReadOnlyMemory<byte> PduDeframer(ReadOnlyMemory<byte> givResponse);
    string WifiHost { get; }
    string SerialNo { get; }
    int RegisterAddress { get; }
}
