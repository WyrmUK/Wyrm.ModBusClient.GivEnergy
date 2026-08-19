using Moq;
using Shouldly;
using Wyrm.ModBusClient.GivEnergy.Services;

namespace Wyrm.ModBusClient.GivEnergy.UnitTests.Services;

public class FramerServiceTests
{
    #region Setup

    private static readonly byte[] CheckSumData = [101, 102];

    private readonly ICheckSumService _checkSumService = Mock.Of<ICheckSumService>();

    private readonly IFramerService _framerService;

    public FramerServiceTests()
    {
        _framerService = new FramerService(_checkSumService);
        Mock.Get(_checkSumService)
            .Setup(x => x.CheckSum(It.IsAny<byte[]>()))
            .Returns(CheckSumData);
    }

    #endregion

    private static readonly byte[] CheckSum = [100, 200];
    private const byte GivUnitId = 0x01;
    private const byte GivFuncNo = 0x02;
    private static readonly byte[] CommandPadding = new byte[16];

    [Fact]
    public void PduFramer_Should_Frame_PDU()
    {
        List<byte> command = [1, 2, 3, 4, 5, 6, 7, 8];
        var commandCount = command.Count + 2;
        Mock.Get(_checkSumService)
            .Setup(x => x.CheckSum(command))
            .Returns(CheckSum);
        var result = _framerService.PduFramer(command);
        result.ShouldBeEquivalentTo(new List<byte>([GivUnitId, GivFuncNo, .. CommandPadding, (byte)(commandCount >> 8), (byte)(commandCount & 0xff), .. command, .. CheckSum]));
    }

    [Fact]
    public void PduDeFramer_Should_Throw_GivEnergyClientException_If_Heartbeat()
    {
        var response = new ReadOnlyMemory<byte>([1, 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 1]);
        var exception = Should.Throw<GivEnergyClientException>(() => _framerService.PduDeframer(response));
        exception.Message.ShouldBe("Heartbeat received.");
        exception.ErrorData.ToArray().ShouldBeEquivalentTo(response.ToArray());
    }

    [Fact]
    public void PduDeFramer_Should_Throw_GivEnergyClientException_If_Insufficient_Register_Data()
    {
        var response = new ReadOnlyMemory<byte>([.. Enumerable.Range(1, 38).Select(i => (byte)i)]);
        var exception = Should.Throw<GivEnergyClientException>(() => _framerService.PduDeframer(response));
        exception.Message.ShouldBe("Insufficient register data.");
        exception.ErrorData.ToArray().ShouldBeEquivalentTo(response.ToArray());
    }

    [Fact]
    public void PduDeFramer_Should_Throw_GivEnergyClientException_If_Bad_Checksum()
    {
        var response = new ReadOnlyMemory<byte>([.. Enumerable.Range(1, 39).Select(i => (byte)i)]);
        var exception = Should.Throw<GivEnergyClientException>(() => _framerService.PduDeframer(response));
        exception.Message.ShouldBe("Bad checksum received.");
        exception.ErrorData.ToArray().ShouldBeEquivalentTo(response.ToArray());
    }

    [Fact]
    public void PduDeframer_Should_Deframe_PDU()
    {
        var wifiHost = "WH12345678";
        var serialNo = "SN12345678";
        const int registerAddress = 60;
        const int registerBytes = 120;
        var response = new ReadOnlyMemory<byte>([59, 59, .. wifiHost.Select(x => (byte)x), 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, .. serialNo.Select(x => (byte)x), 0, (byte)registerAddress, 0, 60, .. Enumerable.Range(1, registerBytes).Select(x => (byte)x), .. CheckSumData]);
        var result = _framerService.PduDeframer(response);
        _framerService.WifiHost.ShouldBe(wifiHost);
        _framerService.SerialNo.ShouldBe(serialNo);
        _framerService.RegisterAddress.ShouldBe(registerAddress);
        var expected = new List<byte>();
        expected.AddRange(response.Slice(20, 2).Span);
        expected.Add(registerBytes);
        expected.AddRange(response.Slice(36, registerBytes).Span);
        result.ToArray().ShouldBeEquivalentTo(expected.ToArray());
    }

    [Fact]
    public void PduDeFramer_Should_Throw_GivEnergyClientException_If_Error_Deframing()
    {
        var response = new ReadOnlyMemory<byte>([.. Enumerable.Range(200, 37).Select(i => (byte)i), .. CheckSumData]);
        var exception = Should.Throw<GivEnergyClientException>(() => _framerService.PduDeframer(response));
        exception.Message.ShouldBe($"Error decoding data frame: {string.Join(' ', response.ToArray().Select(b => $"{b:X2}"))}");
        exception.InnerException.ShouldNotBeNull();
    }
}
