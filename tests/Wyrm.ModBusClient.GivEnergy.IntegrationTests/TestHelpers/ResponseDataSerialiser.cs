using System.Text.Json;
using Wyrm.ModBusClient.GivEnergy.Responses;
using Xunit.Sdk;

namespace Wyrm.ModBusClient.GivEnergy.IntegrationTests.TestHelpers;

public class ResponseDataSerialiser : XunitSerializer<IResponseData>
{
    public override IResponseData Deserialize(Type type, string serializedValue) =>
        (IResponseData)(JsonSerializer.Deserialize(serializedValue, type) ?? throw new ArgumentNullException(nameof(serializedValue)));

    public override string Serialize(IResponseData value) =>
        JsonSerializer.Serialize(value);
}
