namespace Wyrm.ModBusClient.GivEnergy.Services;

internal class CheckSumService : ICheckSumService
{
    public byte[] CheckSum(ICollection<byte> data)
    {
        ushort crc = 0xFFFF;
        foreach (byte b in data)
        {
            crc ^= b;
            for (int index = 0; index < 8; index++)
            {
                bool lsbSet = (crc & 0x0001) != 0;
                crc >>= 1;

                if (!lsbSet) continue;

                crc ^= 0xA001;
            }
        }

        return [(byte)(crc & 0xff), (byte)(crc >> 8)];
    }
}
