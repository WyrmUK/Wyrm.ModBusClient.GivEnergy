using Shouldly;
using Wyrm.ModBusClient.GivEnergy.Constants;
using Wyrm.ModBusClient.GivEnergy.Extensions;

namespace Wyrm.ModBusClient.GivEnergy.UnitTests.Extensions;

public class UshortExtensionsTests
{
    public static readonly TheoryData<ushort, ushort, uint> UintTests = new()
    {
        { 0, 0, 0 },
        { 0, 1_000, 1_000 },
        { 0, ushort.MaxValue, 65_535 },
        { 100, 0, 6_553_600 },
        { 1_000, 100, 65_536_100 },
        { 10_000, 1_000, 655_361_000 },
        { ushort.MaxValue, ushort.MaxValue, 4_294_967_295 },
    };

    [Theory, MemberData(nameof(UintTests))]
    public void ConvertUint_Should_Return_Correct_Values(ushort value, ushort lowValue, uint expected)
    {
        var result = value.ConvertUint(lowValue);
        result.ShouldBe(expected);
    }

    public static readonly TheoryData<ushort, decimal> SignedTests = new()
    {
        { 0, 0M },
        { 100, 100M },
        { 32_767, 32_767M },
        { 32_768, -32_768M },
        { 50_000, -15_536M },
        { ushort.MaxValue, -1M }
    };

    [Theory, MemberData(nameof(SignedTests))]
    public void ConvertSigned_Should_Return_Correct_Values(ushort value, decimal expected)
    {
        var result = value.ConvertSigned();
        result.ShouldBe(expected);
    }

    public static readonly TheoryData<ushort, decimal> Deci1Tests = new()
    {
        { 0, 0M },
        { 1, 0.1M },
        { 12, 1.2M },
        { 1_003, 100.3M },
        { 20_004, 2_000.4M },
        { ushort.MaxValue, 6_553.5M }
    };

    [Theory, MemberData(nameof(Deci1Tests))]
    public void ConvertDeci_Should_Return_Correct_Values(ushort value, decimal expected)
    {
        var result = value.ConvertDeci();
        result.ShouldBe(expected);
    }

    public static readonly TheoryData<ushort, ushort, decimal> Deci2Tests = new()
    {
        { 0, 0, 0M },
        { 0, 1_001, 100.1M },
        { 0, ushort.MaxValue, 6_553.5M },
        { 100, 6, 655_360.6M },
        { 1_000, 107, 6_553_610.7M },
        { 10_000, 1_008, 65_536_100.8M },
        { ushort.MaxValue, ushort.MaxValue, 429_496_729.5M },
    };

    [Theory, MemberData(nameof(Deci2Tests))]
    public void ConvertDeci_Two_Args_Should_Return_Correct_Values(ushort value, ushort lowValue, decimal expected)
    {
        var result = value.ConvertDeci(lowValue);
        result.ShouldBe(expected);
    }

    public static readonly TheoryData<ushort, decimal> DeciSignedTests = new()
    {
        { 0, 0M },
        { 101, 10.1M },
        { 32_767, 3_276.7M },
        { 32_768, -3_276.8M },
        { 50_000, -1_553.6M },
        { ushort.MaxValue, -0.1M }
    };

    [Theory, MemberData(nameof(DeciSignedTests))]
    public void ConvertDeciSigned_Should_Return_Correct_Values(ushort value, decimal expected)
    {
        var result = value.ConvertDeciSigned();
        result.ShouldBe(expected);
    }

    public static readonly TheoryData<ushort, decimal> Centi1Tests = new()
    {
        { 0, 0M },
        { 1, 0.01M },
        { 12, 0.12M },
        { 1_003, 10.03M },
        { 20_004, 200.04M },
        { ushort.MaxValue, 655.35M }
    };

    [Theory, MemberData(nameof(Centi1Tests))]
    public void ConvertCenti_Should_Return_Correct_Values(ushort value, decimal expected)
    {
        var result = value.ConvertCenti();
        result.ShouldBe(expected);
    }

    public static readonly TheoryData<ushort, ushort, decimal> Centi2Tests = new()
    {
        { 0, 0, 0M },
        { 0, 1_001, 10.01M },
        { 0, ushort.MaxValue, 655.35M },
        { 100, 6, 65_536.06M },
        { 1_000, 107, 655_361.07M },
        { 10_000, 1_008, 6_553_610.08M },
        { ushort.MaxValue, ushort.MaxValue, 42_949_672.95M },
    };

    [Theory, MemberData(nameof(Centi2Tests))]
    public void ConvertCenti_Two_Args_Should_Return_Correct_Values(ushort value, ushort lowValue, decimal expected)
    {
        var result = value.ConvertCenti(lowValue);
        result.ShouldBe(expected);
    }

    public static readonly TheoryData<ushort, decimal> CentiSignedTests = new()
    {
        { 0, 0M },
        { 101, 1.01M },
        { 32_767, 327.67M },
        { 32_768, -327.68M },
        { 50_000, -155.36M },
        { ushort.MaxValue, -0.01M }
    };

    [Theory, MemberData(nameof(CentiSignedTests))]
    public void ConvertCentiSigned_Should_Return_Correct_Values(ushort value, decimal expected)
    {
        var result = value.ConvertCentiSigned();
        result.ShouldBe(expected);
    }

    public static readonly TheoryData<ushort, decimal> Milli1Tests = new()
    {
        { 0, 0M },
        { 1, 0.001M },
        { 12, 0.012M },
        { 1_003, 1.003M },
        { 20_004, 20.004M },
        { ushort.MaxValue, 65.535M }
    };

    [Theory, MemberData(nameof(Milli1Tests))]
    public void ConvertMilli_Should_Return_Correct_Values(ushort value, decimal expected)
    {
        var result = value.ConvertMilli();
        result.ShouldBe(expected);
    }

    public static readonly TheoryData<ushort, ushort, decimal> Milli2Tests = new()
    {
        { 0, 0, 0M },
        { 0, 1_001, 1.001M },
        { 0, ushort.MaxValue, 65.535M },
        { 100, 6, 6_553.606M },
        { 1_000, 107, 65_536.107M },
        { 10_000, 1_008, 655_361.008M },
        { ushort.MaxValue, ushort.MaxValue, 4_294_967.295M },
    };

    [Theory, MemberData(nameof(Milli2Tests))]
    public void ConvertMilli_Two_Args_Should_Return_Correct_Values(ushort value, ushort lowValue, decimal expected)
    {
        var result = value.ConvertMilli(lowValue);
        result.ShouldBe(expected);
    }

    public static TheoryData<ushort, ChargeStatus> ChargeStatusTests()
    {
        var data = new TheoryData<ushort, ChargeStatus>();
        for (ushort index = 0; index < Enum.GetValues<ChargeStatus>().Max(c => (ushort)c) + 1; ++index)
        {
            data.Add(index, index switch
            {
                (ushort)ChargeStatus.Idle => ChargeStatus.Idle,
                (ushort)ChargeStatus.Charging => ChargeStatus.Charging,
                (ushort)ChargeStatus.Finishing => ChargeStatus.Finishing,
                (ushort)ChargeStatus.Discharging => ChargeStatus.Discharging,
                _ => ChargeStatus.Unknown
            });
        }
        return data;
    }

    [Theory, MemberData(nameof(ChargeStatusTests))]
    public void ConvertChargeStatus_Should_Return_Correct_Values(ushort value, ChargeStatus expected)
    {
        var result = value.ConvertChargeStatus();
        result.ShouldBe(expected);
    }

    public static readonly TheoryData<ushort, decimal> PowerFactorTests = new()
    {
        { 0, -1M },
        { 5_000, -0.5M },
        { 7_500, -0.25M },
        { 10_000, 0M },
        { 20_000, 1M },
        { 32767, 2.2767M },
        { 50_000, 4M },
        { ushort.MaxValue, 5.5535M }
    };

    [Theory, MemberData(nameof(PowerFactorTests))]
    public void ConvertPowerFactor_Should_Return_Correct_Values(ushort value, decimal expected)
    {
        var result = value.ConvertPowerFactor();
        result.ShouldBe(expected);
    }

    public static readonly TheoryData<ushort, string> Hex1Tests = new()
    {
        { 0, "0000" },
        { 1, "0001" },
        { 10, "000A" },
        { 100, "0064" },
        { 1_000, "03E8" },
        { 10_000, "2710" },
        { 32_767, "7FFF" },
        { 45_000, "AFC8" },
        { ushort.MaxValue, "FFFF" }
    };

    [Theory, MemberData(nameof(Hex1Tests))]
    public void ConvertHex_Should_Return_Correct_Values(ushort value, string expected)
    {
        var result = value.ConvertHex();
        result.ShouldBe(expected);
    }

    public static readonly TheoryData<ushort, ushort, string> Hex2Tests = new()
    {
        { 0, 0, "00000000" },
        { 0, 1, "00000001" },
        { 0, 10, "0000000A" },
        { 0, 100, "00000064" },
        { 0, 1_000, "000003E8" },
        { 0, 10_000, "00002710" },
        { 0, 32_767, "00007FFF" },
        { 0, 45_000, "0000AFC8" },
        { 0, ushort.MaxValue, "0000FFFF" },
        { 1, 10, "0001000A" },
        { 10, 100, "000A0064" },
        { 100, 1_000, "006403E8" },
        { 1_000, 10_000, "03E82710" },
        { 10_000, 32_767, "27107FFF" },
        { 32_767, 45_000, "7FFFAFC8" },
        { ushort.MaxValue, ushort.MaxValue, "FFFFFFFF" }
    };

    [Theory, MemberData(nameof(Hex2Tests))]
    public void ConvertHex_Two_Args_Should_Return_Correct_Values(ushort value, ushort lowValue, string expected)
    {
        var result = value.ConvertHex(lowValue);
        result.ShouldBe(expected);
    }

    public static readonly TheoryData<ushort, ushort, TimeSpan> TimeSpanHoursTests = new()
    {
        { 0, 0, TimeSpan.FromHours(0) },
        { 0, 1, TimeSpan.FromHours(1) },
        { 0, 10, TimeSpan.FromHours(10) },
        { 0, 100, TimeSpan.FromHours(100) },
        { 0, 1_000, TimeSpan.FromHours(1_000) },
        { 0, 10_000, TimeSpan.FromHours(10_000) },
        { 0, 32_767, TimeSpan.FromHours(32_767) },
        { 0, 45_000, TimeSpan.FromHours(45_000) },
        { 0, ushort.MaxValue, TimeSpan.FromHours(65535) },
        { 1, 10, TimeSpan.FromHours(65_546) },
        { 10, 100, TimeSpan.FromHours(655_460) },
        { 100, 1_000, TimeSpan.FromHours(6_554_600) },
        { 1_000, 10_000, TimeSpan.FromHours(65_546_000) },
        { 3_909, 24_554, TimeSpan.FromHours(256_204_778) },
        { 3_909, 24_555, TimeSpan.MaxValue },
        { 10_000, 32_767, TimeSpan.MaxValue },
        { 32_767, 45_000, TimeSpan.MaxValue },
        { ushort.MaxValue, ushort.MaxValue, TimeSpan.MaxValue }
    };

    [Theory, MemberData(nameof(TimeSpanHoursTests))]
    public void ConvertTimeSpanHours_Should_Return_Correct_Values(ushort value, ushort lowValue, TimeSpan expected)
    {
        var result = value.ConvertTimeSpanHours(lowValue);
        result.ShouldBe(expected);
    }

    public static TheoryData<ushort, List<ChargerWarningCode>> ChargerWarningCodeTests()
    {
        var data = new TheoryData<ushort, List<ChargerWarningCode>>
        {
            { 0, [] }
        };
        ushort value = 0;
        var codes = new List<ChargerWarningCode>();
        foreach (var code in Enum.GetValues<ChargerWarningCode>())
        {
            value |= (ushort)code;
            codes.Add(code);
            data.Add(value, [.. codes]);
        }
        return data;
    }

    [Theory, MemberData(nameof(ChargerWarningCodeTests))]
    public void ConvertChargerWarningCode_Should_Return_Correct_Values(ushort value, List<ChargerWarningCode> expected)
    {
        var result = value.ConvertChargerWarningCode();
        result.ShouldBeEquivalentTo(expected);
    }

    public static readonly TheoryData<ushort, ushort[], string> StringTests = new()
    {
        { 'A' << 8, [], "A" },
        { ('A' << 8) + 'b', [], "Ab" },
        { ('T' << 8) + 'e', [('s' << 8) + 't'], "Test" },
        { ('T' << 8) + 'e', [('s' << 8) + 't', 's' << 8], "Tests" },
        { ('T' << 8) + 'e', [('s' << 8) + 't', ('s' << 8) + '.'], "Tests." }
    };

    [Theory, MemberData(nameof(StringTests))]
    public void ConvertString_Should_Return_Correct_Value(ushort value, ushort[] values, string expected)
    {
        var result = value.ConvertString(values);
        result.ShouldBe(expected);
    }

    public static TheoryData<ushort, InverterModel> ModelTests()
    {
        var data = new TheoryData<ushort, InverterModel>
        {
            { 0x1001, InverterModel.Unknown }
        };
        foreach (var code in Enum.GetValues<InverterModel>())
        {
            data.Add((ushort)code, code);
        }
        data.Add(0xFFFF, InverterModel.Unknown);
        return data;
    }

    [Theory, MemberData(nameof(ModelTests))]
    public void ConvertModel_Should_Return_Correct_Values(ushort value, InverterModel expected)
    {
        var result = value.ConvertModel();
        result.ShouldBe(expected);
    }

    public static readonly TheoryData<ushort, ushort, (TimeOnly Start, TimeOnly End)?> TimeSlotTests = new()
    {
        { 0, 0, null },
        { 60, 60, null },
        { 0, 2359, (new TimeOnly(0, 0), new TimeOnly(23, 59)) },
        { 800, 925, (new TimeOnly(8, 0), new TimeOnly(9, 25)) },
        { 1030, 1345, (new TimeOnly(10, 30), new TimeOnly(13, 45)) },
        { 2400, 2500, (TimeOnly.MaxValue, TimeOnly.MaxValue) },
        { 1460, 1500, (TimeOnly.MaxValue, TimeOnly.MaxValue) },
        { 1400, 2400, (TimeOnly.MaxValue, TimeOnly.MaxValue) },
        { 1400, 1560, (TimeOnly.MaxValue, TimeOnly.MaxValue) }
    };

    [Theory, MemberData(nameof(TimeSlotTests))]
    public void ConvertTimeSlot_Should_Return_Correct_Values(ushort value, ushort lowValue, (TimeOnly Start, TimeOnly End)? expected)
    {
        var result = value.ConvertTimeSlot(lowValue);
        result.ShouldBe(expected);
    }

    public static readonly TheoryData<ushort, ushort, ushort, ushort, ushort, ushort, DateTime> DateTimeTests = new()
    {
        { 26, 8, 2, 22, 38, 43, new DateTime(2026, 8, 2, 22, 38, 43) },
        { 26, 13, 2, 22, 38, 43, DateTime.MinValue },
        { 26, 8, 32, 22, 38, 43, DateTime.MinValue },
        { 26, 8, 2, 24, 38, 43, DateTime.MinValue },
        { 26, 8, 2, 22, 60, 43, DateTime.MinValue },
        { 26, 8, 2, 22, 38, 60, DateTime.MinValue }
    };

    [Theory, MemberData(nameof(DateTimeTests))]
    public void ConvertDateTime_Should_Return_Correct_Values(ushort value, ushort month, ushort day, ushort hour, ushort minute, ushort second, DateTime expected)
    {
        var result = value.ConvertDateTime(month, day, hour, minute, second);
        result.ShouldBe(expected);
    }

    public static TheoryData<ushort, ushort, List<InverterFaultCode>> InverterFaultCodesTests()
    {
        var data = new TheoryData<ushort, ushort, List<InverterFaultCode>>
        {
            { 0, 0, [] },
            { 0, 1, [] }
        };
        ushort value = 0;
        ushort lowValue = 0;
        var codes = new List<InverterFaultCode>();
        foreach (var code in Enum.GetValues<InverterFaultCode>())
        {
            var bitField = 1U << (int)code;
            value |= (ushort)(bitField >> 16);
            lowValue |= (ushort)(bitField & 0xFFFF);
            codes.Add(code);
            data.Add(value, lowValue, [.. codes]);
        }
        return data;
    }

    [Theory, MemberData(nameof(InverterFaultCodesTests))]
    public void ConvertInverterFaultCodes_Should_Return_Correct_Values(ushort value, ushort lowValue, List<InverterFaultCode> expected)
    {
        var result = value.ConvertInverterFaultCodes(lowValue);
        result.ShouldBeEquivalentTo(expected);
    }

    [Fact]
    public void FirmwareVersion_Should_Return_Correct_Value()
    {
        const ushort dspFirmwareVersion = 0x0020;
        const ushort armFirmwareVersion = 0x0132;
        var result = dspFirmwareVersion.FirmwareVersion(armFirmwareVersion);
        result.ShouldBe($"D0.{dspFirmwareVersion}-A0.{armFirmwareVersion}");
    }
}
