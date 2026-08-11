namespace Wyrm.ModBusClient.GivEnergy.Services;

internal interface ICheckSumService
{
    byte[] CheckSum(ICollection<byte> data);
}
