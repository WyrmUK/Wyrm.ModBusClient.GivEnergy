using Shouldly;

namespace Wyrm.ModBusClient.GivEnergy.UnitTests;

public class GivEnergyClientExceptionTests
{
    [Theory, CombinatorialData]
    public void Constructor_Should_Save_ErrorData(bool withInnerException)
    {
        const string message = "Message1";
        var data = new ReadOnlyMemory<byte>([0, 1, 2, 3, 4, 5, 6, 7, 8]);
        var innerException = withInnerException ? new ArgumentException() : null;
        var result = new GivEnergyClientException(message, data, innerException);
        result.Message.ShouldBe(message);
        result.ErrorData.ShouldBe(data);
        result.InnerException.ShouldBe(innerException);
    }

    [Theory, CombinatorialData]
    public void Constructor_Should_Set_Empty_ErrorData(bool withInnerException)
    {
        const string message = "Message1";
        var innerException = withInnerException ? new ArgumentException() : null;
        var result = new GivEnergyClientException(message, innerException);
        result.Message.ShouldBe(message);
        result.ErrorData.ShouldBeEquivalentTo(new ReadOnlyMemory<byte>());
        result.InnerException.ShouldBe(innerException);
    }
}
