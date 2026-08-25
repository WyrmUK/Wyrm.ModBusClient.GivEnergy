using Wyrm.ModBusClient.GivEnergy.IntegrationTests.TestHelpers;
using Wyrm.ModBusClient.GivEnergy.Responses;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(typeof(ResponseDataSerialiser), typeof(IResponseData))]