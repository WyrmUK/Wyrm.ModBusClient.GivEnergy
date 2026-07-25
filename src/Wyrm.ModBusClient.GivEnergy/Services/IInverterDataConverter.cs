namespace Wyrm.ModBusClient.GivEnergy.Services;

internal interface IInverterDataConverter
{
    GivEnergyResponse ParseResponse(string serialNo, string wifiHost, int registerAddress, UshortDataResponse response);
}
