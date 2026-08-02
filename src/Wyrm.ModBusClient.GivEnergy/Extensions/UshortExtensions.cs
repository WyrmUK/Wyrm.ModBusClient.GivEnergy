using System.Text;
using Wyrm.ModBusClient.GivEnergy.Constants;

namespace Wyrm.ModBusClient.GivEnergy.Extensions;

internal static class UshortExtensions
{
    extension(ushort value)
    {
        public uint ConvertUint(ushort lowValue) => ((uint)value << 16) + lowValue;
        public decimal ConvertSigned() => value < 0x8000 ? value : value - 0x10000;
        public decimal ConvertDeci() => value / 10M;
        public decimal ConvertDeci(ushort lowValue) => value.ConvertUint(lowValue) / 10M;
        public decimal ConvertDeciSigned() => value.ConvertSigned() / 10M;
        public decimal ConvertCenti() => value / 100M;
        public decimal ConvertCenti(ushort lowValue) => (((long)value << 16) + lowValue) / 100M;
        public decimal ConvertCentiSigned() => value.ConvertSigned() / 100M;
        public decimal ConvertMilli() => value / 1000M;
        public decimal ConvertMilli(ushort lowValue) => (((long)value << 16) + lowValue) / 1000M;
        public ChargeStatus ConvertChargeStatus() => value switch
        {
            (ushort)Constants.ChargeStatus.Idle => Constants.ChargeStatus.Idle,
            (ushort)Constants.ChargeStatus.Charging => Constants.ChargeStatus.Charging,
            (ushort)Constants.ChargeStatus.Finishing => Constants.ChargeStatus.Finishing,
            (ushort)Constants.ChargeStatus.Discharging => Constants.ChargeStatus.Discharging,
            _ => Constants.ChargeStatus.Unknown
        };
        public decimal ConvertPowerFactor() => (value / 10_000M) - 1;
        public string ConvertHex() => $"{value:X4}";
        public string ConvertHex(ushort lowValue) => $"{(((uint)value) << 16) + lowValue:X8}";
        public TimeSpan ConvertTimeSpanHours(ushort lowValue)
        {
            var hours = value.ConvertUint(lowValue);
            return hours > TimeSpan.MaxValue.TotalHours
                ? TimeSpan.MaxValue
                : TimeSpan.FromHours(hours);
        }
        public ICollection<ChargerWarningCode> ConvertChargerWarningCode()
        {
            if (value == 0) return [];

            var warningCodes = new List<ChargerWarningCode>();
            foreach (var code in Enum.GetValues<ChargerWarningCode>())
            {
                if ((value & (ushort)code) == 0) continue;
                warningCodes.Add(code);
            }
            return warningCodes;
        }
        public string ConvertString(params ushort[] values)
        {
            var str = new StringBuilder();
            str.Append((char)(value >> 8));
            str.Append((char)(value & 0xFF));
            foreach (var val in values)
            {
                str.Append((char)(val >> 8));
                str.Append((char)(val & 0xFF));
            }
            return str.ToString().Trim('\0');
        }
        public InverterModel ConvertModel()
        {
            var model = InverterModel.Unknown;
            foreach (var type in Enum.GetValues<InverterModel>())
            {
                if ((value & 0xF000) != ((ushort)type & 0xF000)) continue;
                var compare = value & (ushort)type;
                if (compare == 0 || compare != (ushort)type) continue;
                model = type;
            }
            return model;
        }
        public (TimeOnly Start, TimeOnly End)? ConvertTimeSlot(ushort lowValue)
        {
            if ((value == 0 && lowValue == 0) || (value == 60 && lowValue == 60)) return null;
            try
            {
                var start = new TimeOnly(value / 100, value % 100);
                var end = new TimeOnly(lowValue / 100, lowValue % 100);
                return (start, end);
            }
            catch (ArgumentOutOfRangeException)
            {
                return (TimeOnly.MaxValue, TimeOnly.MaxValue);
            }
        }
        public DateTime ConvertDateTime(ushort month, ushort day, ushort hour, ushort minute, ushort second)
        {
            try
            {
                return new DateTime(2000 + value, month, day, hour, minute, second);
            }
            catch (ArgumentOutOfRangeException)
            {
                return DateTime.MinValue;
            }
        }
        public ICollection<InverterFaultCode> ConvertInverterFaultCodes(ushort lowValue)
        {
            var codes = new List<InverterFaultCode>();
            var bits = value.ConvertUint(lowValue);
            foreach (var code in Enum.GetValues<InverterFaultCode>())
            {
                var bitField = 1U << (int)code;
                if ((bits & bitField) == 0) continue;
                codes.Add(code);
            }
            return codes;
        }
    }
}
