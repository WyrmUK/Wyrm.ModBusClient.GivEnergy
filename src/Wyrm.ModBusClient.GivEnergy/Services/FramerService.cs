using System.Text;

namespace Wyrm.ModBusClient.GivEnergy.Services;

internal class FramerService(
    ICheckSumService _checkSumService) : IFramerService
{
    private const byte GivUnitId = 0x01;
    private const byte GivFuncNo = 0x02;
    private static readonly byte[] CommandPadding = new byte[16];

    public IList<byte> PduFramer(IList<byte> command)
    {
        var count = command.Count + 2;
        var givCommand = new List<byte>([GivUnitId, GivFuncNo, .. CommandPadding, (byte)(count >> 8), (byte)(count & 0xff)]);
        givCommand.AddRange(command);
        givCommand.AddRange(_checkSumService.CheckSum(command));
        return givCommand;
    }

    public ReadOnlyMemory<byte> PduDeframer(ReadOnlyMemory<byte> givResponse)
    {
        const int wifiHostStartPosition = 2;
        const int unitIdentifierPosition = 20;
        const int serialNoStartPosition = 22;
        const int stringLength = 10;
        const int regAddressHighPosition = 32;
        const int regAddressLowPosition = 33;
        const int regCountHighPosition = 34;
        const int regCountLowPosition = 35;
        const int registerValuesStartPosition = 36;

        var responseSpan = givResponse.Span;
        if (responseSpan.Length == 13 && responseSpan[0] == 1 && responseSpan[1] == 1 && responseSpan[^1] == 1)
            throw new GivEnergyClientException("Heartbeat received.", givResponse);

        // TODO: Check for other responses other than registers

        var numRegisters = (responseSpan[regCountHighPosition] << 8) + responseSpan[regCountLowPosition];
        // TODO: Check number of registers

        var checkSum = _checkSumService.CheckSum(responseSpan[unitIdentifierPosition..^2].ToArray());
        // TODO: Check CheckSum: everything after length but not checksum itself of course

        try
        {
            WifiHost = Encoding.ASCII.GetString([.. givResponse.Slice(wifiHostStartPosition, stringLength).TrimEnd((byte)0).ToArray()]);
            SerialNo = Encoding.ASCII.GetString([.. givResponse.Slice(serialNoStartPosition, stringLength).TrimEnd((byte)0).ToArray()]);
            var response = new List<byte>();
            response.AddRange(givResponse.Slice(unitIdentifierPosition, 2).Span);
            RegisterAddress = (responseSpan[regAddressHighPosition] << 8) + responseSpan[regAddressLowPosition];
            var bytes = numRegisters * 2;
            response.Add((byte)bytes);
            response.AddRange(givResponse.Slice(registerValuesStartPosition, bytes).Span);
            return new ReadOnlyMemory<byte>([.. response]);
        }
        catch (Exception ex)
        {
            throw new GivEnergyClientException($"Error decoding data frame: {string.Join(' ', givResponse.ToArray().Select(b => $"{b:X2}"))}", ex);
        }
    }

    public string WifiHost { get; private set; } = string.Empty;
    public string SerialNo { get; private set; } = string.Empty;
    public int RegisterAddress { get; private set; }
}
