using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Net;
using Wyrm.ModBusClient.GivEnergy.Constants;
using Wyrm.ModBusClient.GivEnergy.DependencyInjection;
using Wyrm.ModBusClient.GivEnergy.Extensions;
using Wyrm.ModBusClient.GivEnergy.Responses;
using Wyrm.ModBusClient.GivEnergy.Responses.Constants;
using Wyrm.ModBusClient.GivEnergy.Responses.Models;
using Wyrm.ModBusClient.GivEnergy.Services;

namespace Wyrm.ModBusClient.GivEnergy.IntegrationTests;

public class GivEnergyClientTests : IDisposable
{
    #region Setup

    private readonly IGivEnergyClient _givEnergyClient;
    private readonly ICheckSumService _checkSumService;
    private readonly System.Net.Sockets.Socket _inverter = new(TestEndPoint.AddressFamily, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);

    private static readonly EndPoint TestEndPoint = new IPEndPoint(new IPAddress([127, 0, 0, 1]), 8000);

    public GivEnergyClientTests()
    {
        var services = new ServiceCollection().AddGivEnergyClient();
        var provider = services.BuildServiceProvider();
        _givEnergyClient = provider.GetRequiredService<IGivEnergyClient>();
        _checkSumService = new CheckSumService();
        _inverter.Bind(TestEndPoint);
        _inverter.Listen();
    }

    public void Dispose()
    {
        _inverter.Close();
        GC.SuppressFinalize(this);
    }

    #endregion

    private const string WifiHost = "WH12345678";
    private const string SerialNo = "SN12345678";

    private static readonly byte[] InverterProperties1Data = [0x20, 0x03, 0x00, 0x03, 0x08, 0x24, 0x00, 0x00, 0x00, 0x00, 0x8c, 0xa0, 0x0e, 0x10, 0x00, 0x01, 0x42, 0x49, 0x31, 0x32, 0x33, 0x34, 0x47, 0x30, 0x31, 0x32, 0x46, 0x41, 0x31, 0x32, 0x33, 0x34, 0x46, 0x31, 0x32, 0x33, 0x0b, 0xce, 0x01, 0x3e, 0x00, 0x01, 0x01, 0x3e, 0x00, 0x02, 0x00, 0x00, 0xc0, 0x00, 0x00, 0x4b, 0x17, 0x70, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11, 0x00, 0x01, 0x00, 0x04, 0x00, 0x07, 0x00, 0x8c, 0x00, 0x1a, 0x00, 0x06, 0x00, 0x1d, 0x00, 0x14, 0x00, 0x26, 0x00, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x65, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x33, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00];
    private static readonly InverterProperties1 InverterProperties1Response = new()
    {
        DeviceTypeCode = "2003",
        Model = InverterModel.HybridGen3,
        Module = "00030824",
        NumberOfMPPT = 0,
        NumberOfPhases = 0,
        Reg005 = 0,
        Reg006 = 36000,
        Reg007 = 3600,
        EnableAmmeter = true,
        UnusedSerialNumber = "BI1234G012",
        SerialNumber = "FA1234F123",
        FirstBatteryBMSFirmwareVersion = 3022,
        DSPFirmwareVersion = 318,
        EnableChargeTarget = true,
        ARMFirmwareVersion = 318,
        UsbDeviceType = UsbDeviceType.Disk,
        SelectARMChip = false,
        VariableAddress = 49152,
        VariableValue = 75,
        GridPortMaximumPowerOutput = 6000,
        BatteryPowerMode = BatteryPowerMode.SelfConsumption,
        Enable60HzFrequencyMode = false,
        BatteryCalibrationStage = BatteryCalibrationStage.Off,
        ModbusAddress = 17,
        ChargeSlot2 = (new TimeOnly(0, 1), new TimeOnly(0, 4)),
        UserCode = 7,
        ModbusVersion = "1.40",
        SystemTime = new DateTime(2026, 6, 29, 20, 38, 24),
        EnableDRMRJ45Port = false,
        EnableReversedCTClamp = false,
        ChargeState = 0,
        DischargeState = 0,
        DischargeSlot2 = null,
        BMSFirmwareVersion = 101,
        MeterType = MeterType.EM115,
        EnableReversed115Meter = false,
        EnableReversed418Meter = false,
        ActivePowerRate = 100,
        ReactivePowerRate = 0,
        PowerFactor = -1M,
        EnableInverterAutoRestart = false,
        EnableInverter = true,
        BatteryType = BatteryType.Lithium,
        BatteryCapacity = 51,
        DischargeSlot1 = null,
        EnableAutoJudgeBatteryType = true,
        EnableDischarge = false
    };
    private static readonly byte[] InverterProperties2Data = [0x05, 0x14, 0x00, 0x1e, 0x00, 0x1e, 0x07, 0x30, 0x0a, 0x3e, 0x12, 0x8e, 0x14, 0x50, 0x00, 0x7d, 0x00, 0x32, 0x03, 0xe8, 0x00, 0x19, 0x07, 0x08, 0x0a, 0xaa, 0x12, 0x5c, 0x14, 0x50, 0x00, 0x7a, 0x00, 0x19, 0x00, 0x19, 0x00, 0x19, 0x07, 0x58, 0x0a, 0x14, 0x12, 0x98, 0x14, 0x46, 0x0a, 0x3e, 0x53, 0x42, 0x31, 0x2e, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xae, 0x02, 0x8f, 0x00, 0x01, 0x10, 0xe0, 0x16, 0xda, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x01, 0x00, 0x05, 0x00, 0x32, 0x00, 0x32, 0x00, 0x00, 0x00, 0x05, 0x00, 0x01, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];
    private static readonly InverterProperties2 InverterProperties2Response = new()
    {
        PVStartVoltage = 130M,
        StartCountdownTimer = 30,
        RestartDelayTime = 30,
        ACLowLimitTripVoltage = 184M,
        ACHighLimitTripVoltage = 262.2M,
        ACLowLimitTripFrequency = 47.5M,
        ACHighLimitTripFrequency = 52M,
        ACLowVoltageTripTime = 1.25M,
        ACHighVoltageTripTime = 0.5M,
        ACLowFrequencyTripTime = 10M,
        ACHighFrequencyTripTime = 0.25M,
        ACLowLimitReconnectVoltage = 180M,
        ACHighLimitReconnectVoltage = 273M,
        ACLowLimitReconnectFrequency = 47M,
        ACHighLimitReconnectFrequency = 52M,
        ACLowVoltageReconnectTime = 1.22M,
        ACHighVoltageReconnectTime = 0.25M,
        ACLowFrequencyReconnectTime = 0.25M,
        ACHighFrequencyReconnectTime = 0.25M,
        ACLowLimitGridVoltage = 188M,
        ACHighLimitGridVoltage = 258M,
        ACLowLimitGridFrequency = 47.6M,
        ACHighLimitGridFrequency = 51.9M,
        AC10MinuteProtectVoltage = 262.2M,
        ISOProtection1 = 21314,
        ISOProtection2 = 12590,
        GFCIProtectionValue1 = 12288,
        GFCIProtectionTime1 = 0,
        GFCIProtectionValue2 = 0,
        GFCIProtectionTime2 = 0,
        DCIProtectionValue1 = 0,
        DCIProtectionTime1 = 0,
        DCIProtectionValue2 = 0,
        DCIProtectionTime2 = 0,
        ChargeSlot1 = (new TimeOnly(4, 30), new TimeOnly(6, 55)),
        EnableCharge = true,
        BatteryLowVoltageProtectionLimit = 43.2M,
        BatteryHighVoltageProtectionLimit = 58.5M,
        String1VoltageAdjustment = 0,
        String2VoltageAdjustment = 0,
        GridImportLimit = 100,
        GridImportLimitEnabled = false,
        EnableLORA = false,
        EnableBatterySelfHeating = false,
        BatteryVoltageAdjust = 0M,
        String1PowerAdjustment = 0,
        String2PowerAdjustment = 0,
        BatteryLowForceChargeTime = 6,
        EnableBMSRead = true,
        BatteryStateOfChargeReserve = 5,
        BatteryChargeLimit = 50,
        BatteryDischargeLimit = 50,
        EnableBuzzer = false,
        BatteryDischargeMinPowerReserve = 5,
        Reg116 = 1,
        ChargeTargetStateOfCharge = 100,
        ChargeStateOfChargeStop2 = 0,
        DischargeStateOfChargeStop2 = 0,
        ChargeStateOfChargeStop1 = 0
    };
    private static readonly byte[] InverterProperties3Data = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x18, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0xff, 0x4e, 0x20, 0x00, 0xff, 0x4e, 0x20, 0x00, 0xff, 0x4e, 0x20, 0x00, 0xff, 0x4e, 0x20, 0x09, 0xb4, 0x09, 0xe2, 0x08, 0x44, 0x08, 0x16, 0x00, 0x14, 0x00, 0x05, 0x09, 0x6f, 0x08, 0xfc, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0f, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];
    private static readonly InverterProperties3 InverterProperties3Response = new()
    {
        DischargeStateOfChargeStop1 = 0,
        EnableLocalCommandTest = false,
        PowerFactorFunction = PowerFactorFunction.PF1,
        FrequencyLoadLimitRate = 24,
        EnableLowVoltageFaultRideThrough = false,
        EnableFrequencyDerating = true,
        EnableAbove6kWSystem = false,
        StartSystemAutoTest = false,
        EnableSPI = true,
        PowerFactorCommandMemoryState = 1,
        PowerFactorPoints = [(255, 20000), (255, 20000), (255, 20000), (255, 20000)],
        CEI021V1SQuotient = 248.4M,
        CEI021V2SQuotient = 253M,
        CEI021V1LQuotient = 211.6M,
        CEI021V2LQuotient = 207M,
        CEI021LockInActivePower = 20,
        CEI021LockOutActivePower = 5,
        CEI021LockInGridVoltage = 241.5M,
        CEI021LockOutGridVoltage = 230M,
        LVFRTReactiveRate = 0,
        LVFRTLowFaults = [(0, 0), (0, 0), (0, 0), (0, 0)],
        LVFRTHighFaults = [(0, 0)],
        Reg158 = 0,
        Reg159 = 0,
        Reg160 = 0,
        Reg161 = 0,
        Reg162 = 0,
        ResetUserInformation = 0,
        InverterReboot = 0,
        Reg165 = 0,
        Reg166 = 0,
        EnableRealTimeControl = false,
        ThreePhaseBalanceMode = 0,
        ThreePhaseABC = 0,
        ThreePhaseBalance1 = 0,
        ThreePhaseBalance2 = 0,
        ThreePhaseBalance3 = 0,
        Reg173 = 0,
        Reg174 = 0,
        Reg175 = 0,
        EnableBatteryOnPVOrGrid = false,
        DebugInverter = 15,
        EnableUPSMode = false,
        EnableG100LimitSwitch = false,
        EnableBatteryCableImpedanceAlarm = false
    };
    private static readonly byte[] InverterProperties4Data = [0x58, 0x58, 0x58, 0x58, 0x58, 0x58, 0x58, 0x58, 0x58, 0x58, 0x58, 0x58, 0x58, 0x58, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];
    private static readonly InverterProperties4 InverterProperties4Response = new()
    {
        Reg181To199 = [22616, 22616, 22616, 22616, 22616, 22616, 22616, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
        EnableInverterParallelMode = false,
        CommandBMDFlashUpdate = false,
        Reg202 = 0,
        Reg203 = 0,
        InverterErrors = 0,
        InverterFaultCodes = [],
        Reg206To240 = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0]
    };
    private static readonly byte[] InverterProperties5Data = [0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04];
    private static readonly InverterProperties5 InverterProperties5Response = new()
    {
        Reg241 = 0,
        Reg242 = 0,
        ChargeTargetStateOfCharge1 = 100,
        ChargeSlot2X = null,
        ChargeTargetStateOfCharge2 = 100,
        ChargeSlot3 = null,
        ChargeTargetStateOfCharge3 = 100,
        ChargeSlot4 = null,
        ChargeTargetStateOfCharge4 = 100,
        ChargeSlot5 = null,
        ChargeTargetStateOfCharge5 = 100,
        ChargeSlot6 = null,
        ChargeTargetStateOfCharge6 = 100,
        ChargeSlot7 = null,
        ChargeTargetStateOfCharge7 = 100,
        ChargeSlot8 = null,
        ChargeTargetStateOfCharge8 = 100,
        ChargeSlot9 = null,
        ChargeTargetStateOfCharge9 = 100,
        ChargeSlot10 = null,
        ChargeTargetStateOfCharge10 = 100,
        Reg271 = 0,
        Reg272 = 0,
        DischargeTargetStateOfCharge1 = 4,
        Reg274 = 0,
        Reg275 = 0,
        DischargeTargetStateOfCharge2 = 4,
        DischargeSlot3 = null,
        DischargeTargetStateOfCharge3 = 4,
        DischargeSlot4 = null,
        DischargeTargetStateOfCharge4 = 4,
        DischargeSlot5 = null,
        DischargeTargetStateOfCharge5 = 4,
        DischargeSlot6 = null,
        DischargeTargetStateOfCharge6 = 4,
        DischargeSlot7 = null,
        DischargeTargetStateOfCharge7 = 4,
        DischargeSlot8 = null,
        DischargeTargetStateOfCharge8 = 4,
        DischargeSlot9 = null,
        DischargeTargetStateOfCharge9 = 4,
        DischargeSlot10 = null,
        DischargeTargetStateOfCharge10 = 4
    };
    private static readonly byte[] InverterProperties6Data = [0x00, 0x0a, 0x00, 0x96, 0x13, 0x88, 0x3a, 0x98, 0x00, 0x00, 0x00, 0x00, 0x17, 0x70, 0x08, 0xfc, 0x0e, 0x10, 0x00, 0x41, 0x00, 0x64, 0x00, 0x00, 0x3a, 0x98, 0x00, 0x64, 0x00, 0x64, 0x04, 0x7e, 0x0a, 0xc8, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x13, 0xb0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x13, 0x74, 0x00, 0x28, 0x13, 0x92, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2e, 0xe0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];
    private static readonly InverterProperties6 InverterProperties6Response = new()
    {
        EnablePlantMode = true,
        PlantRole = 150,
        PlantMeters = 5000,
        OverFrequencyLoadDropRecoveryDelay = 15000,
        Reg305 = 0,
        MPPTOperatingMode = 0,
        ConnectionLoadingSlope = 6000,
        EPSNominalVoltage = 230M,
        BatteryNominalPower = 3600M,
        BatteryNominalCurrent = 65M,
        BatteryMaxChargePercentage = 100M,
        ExportPriority = 0,
        UnderFrequencyAddLoadDelay = 15000,
        BatteryChargeLimitAC = 100,
        BatteryDischargeLimitAC = 100,
        EN50549ZeroCurrentLowerVoltageLimit = 115M,
        EN50549ZeroCurrentUpperVoltageLimit = 276M,
        EnableEPS = true,
        BatteryPauseMode = 0,
        BatteryPauseSlot1 = null,
        OverFrequencyDeratingStartPoint = 50.4M,
        EnableTariffPricingBatteryLogic = false,
        ImportPriceBatteryDischargeThreshold = 0,
        ImportPriceBatteryChargeThreshold = 0,
        ExportPriceBatteryDischargeThreshold = 0,
        UnderFrequencyDeratingStartPoint = 49.8M,
        UnderFrequencyLoadingSlope = 40,
        OverFrequencyDeratingStopPoint = 50.1M,
        EnableBMSOCVCalibration = false,
        GatewayPowerOffSetting = 0,
        ForceOffGrid = false,
        EnableMicroGrid = false,
        EnableEVCharger = false,
        EVChargerImportLimit = 0,
        EVChargerReconnectionWaitTime = 0,
        EVChargerStateOfChargeLimit = 0,
        EnableFan = false,
        FanSpeed = 0,
        EnableGateway = false,
        BMSCommunicationMode = 0,
        NPERelayToggle = 0,
        AFCISetting = 0,
        EnableGenerator = false,
        GeneratorStartStateOfCharge = 0,
        GeneratorStopStateOfCharge = 0,
        GeneratorChargePower = 0,
        DisableLEDs = false,
        LCDScreenIdleTimeout = 0,
        LeadAcidBatteryCalibrationUpperLimit = 0M,
        LeadAcidbatteryCalibrationLowerLimit = 0M,
        InverterOperatingMode = 0,
        Reg353 = 0,
        Reg354 = 12000,
        Reg355 = 0,
        Reg356 = 0,
        Reg357 = 0,
        Reg358 = 0,
        Reg359 = 0,
        Reg360 = 0
    };
    private static readonly byte[] InverterData1Data = [0x00, 0x01, 0x04, 0x76, 0x00, 0x00, 0x10, 0xb8, 0x00, 0x00, 0x09, 0x70, 0x00, 0x00, 0xb0, 0xf3, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0xdb, 0x67, 0x13, 0x84, 0x00, 0x01, 0x0b, 0x4e, 0x0c, 0xe4, 0x00, 0x71, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x43, 0xcb, 0x00, 0x00, 0xff, 0xd0, 0x00, 0x1c, 0x00, 0x80, 0x00, 0x00, 0x45, 0x37, 0x00, 0x00, 0xfe, 0x3e, 0x00, 0x00, 0x00, 0x03, 0x25, 0x6e, 0x00, 0x00, 0x00, 0x17, 0x00, 0x1a, 0x00, 0x1a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xc0, 0x01, 0x92, 0x02, 0xad, 0x00, 0x7d, 0x00, 0x00, 0xf0, 0xa1, 0x00, 0x00, 0x65, 0x9b, 0x00, 0x01, 0x14, 0x69, 0x00, 0x00, 0x00, 0x00, 0x09, 0x73, 0x13, 0x85, 0x01, 0xba, 0x01, 0x4a, 0x00, 0x00, 0x01, 0x14, 0x00, 0x04];
    private static readonly InverterData1 InverterData1Response = new()
    {
        Status = GivEnergyStatus.Normal,
        PV1Voltage = 114.2M,
        PV2Voltage = 0M,
        PBusVoltage = 428M,
        NBusVoltage = 0M,
        GridVoltage = 241.6M,
        BatteryThroughput = 4529.9M,
        PV1InputCurrent = 0M,
        PV2InputCurrent = 0M,
        GridOutputCurrent = 0.06M,
        PVGeneratingCapacityTotal = 5616.7M,
        GridFrequency = 49.96M,
        ChargeStatus = 1,
        ChargeStatusType = ChargeStatus.Charging,
        HighbrighBusVoltage = 289.4M,
        InverterOutputPowerFactorNow = -0.67M,
        PV1EnergyToday = 11.3M,
        PV1InputPower = 1M,
        PV2EnergyToday = 0M,
        PV2InputPower = 0M,
        GridOutEnergyTotal = 1735.5M,
        PVSolarDiverterEnergy = 0M,
        GridPowerPH1 = -48M,
        GridOutEnergyToday = 2.8M,
        GridInEnergyToday = 12.8M,
        InverterInEnergyTotal = 1771.9M,
        DischargeEnergyYear = 0M,
        GridPowerAtMeter = -450M,
        BackupPower = 0M,
        GridInEnergyTotal = 20619M,
        Reg0034 = 0,
        ACChargeEnergyToday = 2.3M,
        BatteryChargeEnergyTodayAlt1 = 2.6M,
        BatteryDischargeEnergyTodayAlt1 = 2.6M,
        Countdown = 0,
        InverterFaultCode = "0000",
        InverterWarningCode = "0000",
        InverterHeatsinkTemperature = 44.8M,
        LoadPowerDemand = 402M,
        GridPowerApparent = 685M,
        PVGenerationEnergyToday = 12.5M,
        PVGenerationEnergyTotal = 6160.1M,
        WorkTimeTotal = new TimeSpan(1083, 19, 0, 0),
        SystemMode = 1,
        BatteryVoltage = 52.25M,
        BatteryCurrent = 0M,
        BatteryPower = 0M,
        AC1OutputVoltage = 241.9M,
        AC1OutputFrequency = 49.97M,
        ChargerTemperature = 44.2M,
        BatteryTemperature = 33M,
        ChargerWarningCode = 0,
        ChargerWarningMessages = [],
        GridPortCurrent = 2.76M,
        BatteryPercentage = 4M
    };
    private static readonly byte[] InverterData5Data = [0x09, 0x6f, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5a, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];
    private static readonly InverterData5 InverterData5Response = new()
    {
        Voltage5 = 241.5M,
        Current5 = 0.9M,
        CombinedGenerationPower = 0.1M
    };
    private static readonly byte[] LowVoltageBCUData2Data = [0x00, 0x00, 0x00, 0x00, 0x00, 0x19, 0x00, 0x19, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];
    private static readonly LowVoltageBCUData2 LowVoltageBCUData2Response = new()
    {
        BMSStatus1 = 0,
        BMSStatus2 = 0,
        RequestChargeCurrent = 25,
        RequestDischargeCurrent = 25
    };
    private static readonly byte[] BatteryData20Data = [0x0c, 0xb2, 0x0c, 0xb5, 0x0c, 0xb6, 0x0c, 0xb6, 0x0c, 0xbb, 0x0c, 0xb6, 0x0c, 0xb8, 0x0c, 0xb8, 0x0c, 0xbb, 0x0c, 0xbb, 0x0c, 0xbd, 0x0c, 0xbd, 0x0c, 0xbd, 0x0c, 0xbd, 0x0c, 0xbe, 0x0c, 0xbe, 0x01, 0x47, 0x01, 0x4c, 0x01, 0x4c, 0x01, 0x48, 0xcb, 0x9c, 0x01, 0x46, 0x00, 0x00, 0xcb, 0xce, 0x00, 0x00, 0x12, 0xe8, 0x00, 0x00, 0x13, 0xec, 0x00, 0x00, 0x02, 0xb4, 0x00, 0x00, 0x0e, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xff, 0xe3, 0x04, 0x75, 0x00, 0x10, 0x0b, 0xce, 0x00, 0x00, 0x00, 0x0e, 0x00, 0x00, 0x13, 0xec, 0x01, 0x4c, 0x01, 0x47, 0x5c, 0xef, 0x54, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x42, 0x49, 0x31, 0x32, 0x33, 0x34, 0x47, 0x30, 0x31, 0x32, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];
    private static readonly BatteryData2 BatteryData20Response = new()
    {
        CellVoltages = [3.25M, 3.253M, 3.254M, 3.254M, 3.259M, 3.254M, 3.256M, 3.256M, 3.259M, 3.259M, 3.261M, 3.261M, 3.261M, 3.261M, 3.262M, 3.262M],
        CellsTemperature = [32.7M, 33.2M, 33.2M, 32.8M],
        CellVoltageSum = 5212.4M,
        BMSMosfetTemperature = 32.6M,
        OutVoltage = 52.174M,
        CalibratedCapacity = 48.4M,
        DesignCapacity = 51M,
        RemainingCapacity = 6.92M,
        Status1 = 0,
        Status2 = 0,
        Status3 = 14,
        Status4 = 16,
        Status5 = 0,
        Status6 = 0,
        Status7 = 0,
        Warning1 = 0,
        Warning2 = 0,
        Current = -0.29M,
        NumberOfCycles = 1141,
        NumberofCells = 16,
        BMSFirmwareVersion = 3022,
        Reg160 = 0,
        StateOfCharge = 14,
        DesignCapacity2 = 51M,
        MaximumTemperature = 33.2M,
        MinimumTemperature = 32.7M,
        DischargeTotalEnergy = 2379.1M,
        ChargeTotalEnergy = 2150.8M,
        ForceDischargeFlag = 0,
        Reg169 = 0,
        Reg170 = 0,
        SerialNumber = "BI1234G012",
        UsbDevice = 0,
        Reg177 = 0,
        Reg178 = 0,
        Reg179 = 0,
        Reg180 = 0
    };
    private static readonly byte[] BatteryData21Data = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5c, 0xef, 0x54, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];
    private static readonly BatteryData2 BatteryData21Response = new()
    {
        CellVoltages = [0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M],
        CellsTemperature = [0M, 0M, 0M, 0M],
        CellVoltageSum = 0M,
        BMSMosfetTemperature = 0M,
        OutVoltage = 0M,
        CalibratedCapacity = 0M,
        DesignCapacity = 0M,
        RemainingCapacity = 0M,
        Status1 = 0,
        Status2 = 0,
        Status3 = 0,
        Status4 = 0,
        Status5 = 0,
        Status6 = 0,
        Status7 = 0,
        Warning1 = 0,
        Warning2 = 0,
        Current = 0M,
        NumberOfCycles = 0,
        NumberofCells = 0,
        BMSFirmwareVersion = 0,
        Reg160 = 0,
        StateOfCharge = 0,
        DesignCapacity2 = 0M,
        MaximumTemperature = 0M,
        MinimumTemperature = 0M,
        DischargeTotalEnergy = 2379.1M,
        ChargeTotalEnergy = 2150.8M,
        ForceDischargeFlag = 0,
        Reg169 = 0,
        Reg170 = 0,
        SerialNumber = string.Empty,
        UsbDevice = 0,
        Reg177 = 0,
        Reg178 = 0,
        Reg179 = 0,
        Reg180 = 0
    };
    private static readonly byte[] MeterData20Data = [0x09, 0x74, 0x00, 0x00, 0x00, 0x00, 0x01, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xb2, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x95, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0d, 0x2a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x13, 0x83, 0x25, 0x6e, 0x00, 0x00, 0x43, 0xcb, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];
    private static readonly MeterData2 MeterData20Response = new()
    {
        Phase1 = new MeterPhaseData { Voltage = 242M, Current = 2.72M, ActivePower = 434M, ReactivePower = 0M, ApparentPower = 66.1M, PowerFactor = 0.337M },
        Phase2 = new MeterPhaseData { Voltage = 0M, Current = 0M, ActivePower = 0M, ReactivePower = 0M, ApparentPower = 0M, PowerFactor = 0M },
        Phase3 = new MeterPhaseData { Voltage = 0M, Current = 0m, ActivePower = 0m, ReactivePower = 0M, ApparentPower = 0M, PowerFactor = 0M },
        LineCurrent = 0M,
        TotalCurrent = 0M,
        ActiveTotalPower = 0M,
        ReactiveTotalPower = 0M,
        ApparentTotalPower = 0M,
        TotalPowerFactor = 0M,
        Frequency = 49.95M,
        ActiveImportEnergy = 958.2M,
        ReactiveImportEnergy = 0M,
        ActiveExportEnergy = 1735.5M,
        ReactiveExportEnergy = 0M
    };
    private static readonly byte[] MeterData21Data = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];
    private static readonly MeterData2 MeterData21Response = new()
    {
        Phase1 = new MeterPhaseData { Voltage = 0M, Current = 0M, ActivePower = 0M, ReactivePower = 0M, ApparentPower = 0M, PowerFactor = 0M },
        Phase2 = new MeterPhaseData { Voltage = 0M, Current = 0M, ActivePower = 0M, ReactivePower = 0M, ApparentPower = 0M, PowerFactor = 0M },
        Phase3 = new MeterPhaseData { Voltage = 0M, Current = 0m, ActivePower = 0m, ReactivePower = 0M, ApparentPower = 0M, PowerFactor = 0M },
        LineCurrent = 0M,
        TotalCurrent = 0M,
        ActiveTotalPower = 0M,
        ReactiveTotalPower = 0M,
        ApparentTotalPower = 0M,
        TotalPowerFactor = 0M,
        Frequency = 0M,
        ActiveImportEnergy = 0M,
        ReactiveImportEnergy = 0M,
        ActiveExportEnergy = 0M,
        ReactiveExportEnergy = 0M
    };

    public static readonly TheoryData<ResponseDataType, byte, byte[], IResponseData> RequestInverterTests = new()
    {
        { ResponseDataType.InverterProperties1, 0, InverterProperties1Data, InverterProperties1Response },
        { ResponseDataType.InverterProperties2, 0, InverterProperties2Data, InverterProperties2Response },
        { ResponseDataType.InverterProperties3, 0, InverterProperties3Data, InverterProperties3Response },
        { ResponseDataType.InverterProperties4, 0, InverterProperties4Data, InverterProperties4Response },
        { ResponseDataType.InverterProperties5, 0, InverterProperties5Data, InverterProperties5Response },
        { ResponseDataType.InverterProperties6, 0, InverterProperties6Data, InverterProperties6Response },
        { ResponseDataType.InverterData1, 0, InverterData1Data, InverterData1Response },
        { ResponseDataType.InverterData5, 0, InverterData5Data, InverterData5Response },
        { ResponseDataType.LowVoltageBCUData2, 0, LowVoltageBCUData2Data, LowVoltageBCUData2Response },
        { ResponseDataType.BatteryData2, 0, BatteryData20Data, BatteryData20Response },
        { ResponseDataType.BatteryData2, 1, BatteryData21Data, BatteryData21Response },
        { ResponseDataType.MeterData2, 0, MeterData20Data, MeterData20Response },
        { ResponseDataType.MeterData2, 1, MeterData21Data, MeterData21Response }
    };

    [Theory, MemberData(nameof(RequestInverterTests))]
    public async Task RequestInverterDataAsync_Should_GetInverterData(ResponseDataType responseDataType, byte deviceIndex, IEnumerable<byte> registerData, IResponseData expected)
    {
        var deviceAddress = responseDataType.DeviceAddress(deviceIndex);
        var functionNo = (byte)(responseDataType.InputRegisters() ? 4 : 3);
        var registerAddress = responseDataType.StartAddress();

        var inverterSocket = await ConnectAndAcceptAsync(TestContext.Current.CancellationToken);
        await _givEnergyClient.RequestInverterDataAsync(responseDataType, deviceIndex, TestContext.Current.CancellationToken);
        await ReceiveCommandAndCheckAsync(inverterSocket, deviceAddress, functionNo, registerAddress, TestContext.Current.CancellationToken);

        await SendResponseAsync(inverterSocket, deviceAddress, functionNo, registerAddress, registerData, TestContext.Current.CancellationToken);
        var response = await _givEnergyClient.WaitForResponseAsync(TestContext.Current.CancellationToken);

        _givEnergyClient.Close();

        response.SerialNumber.ShouldBe(SerialNo);
        response.WifiAdapter.ShouldBe(WifiHost);
        response.DeviceNumber.ShouldBe((byte)(deviceIndex + 1));
        response.ResponseDataType.ShouldBe(responseDataType);
        response.ResponseData.ShouldBeOfType(expected.GetType());
        response.ResponseData.ShouldBeEquivalentTo(expected);
    }

    private async Task<System.Net.Sockets.Socket> ConnectAndAcceptAsync(CancellationToken cancellationToken)
    {
        var accept = _inverter.AcceptAsync(cancellationToken);
        await _givEnergyClient.ConnectAsync(TestEndPoint, cancellationToken);
        return await accept;
    }

    private async Task ReceiveCommandAndCheckAsync(System.Net.Sockets.Socket inverterSocket, byte deviceAddress, byte functionNo, ushort registerAddress, CancellationToken cancellationToken)
    {
        var buffer = new byte[1024];
        await inverterSocket.ReceiveAsync(buffer, cancellationToken);
        var expectedRequestData = new byte[] { deviceAddress, functionNo, (byte)(registerAddress >> 8), (byte)(registerAddress & 0xFF), 0, 60 };
        var checkSumData = _checkSumService.CheckSum(expectedRequestData);
        byte[] expectedRequest = [89, 89, 0, 1, 0, 28, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, .. expectedRequestData, .. checkSumData];
        byte[] bufferCheck = [.. expectedRequest, .. Enumerable.Range(1, 1024 - expectedRequest.Length).Select(_ => (byte)0)];
        buffer.ShouldBeEquivalentTo(bufferCheck);
    }

    private async Task SendResponseAsync(System.Net.Sockets.Socket inverterSocket, byte deviceAddress, byte functionNo, ushort registerAddress, IEnumerable<byte> registerData, CancellationToken cancellationToken)
    {
        byte[] pduData = [deviceAddress, functionNo, .. SerialNo.Select(x => (byte)x), (byte)(registerAddress >> 8), (byte)(registerAddress & 0xFF), 0, 60, .. registerData];
        var checkSumData = _checkSumService.CheckSum(pduData);
        byte[] responseData = [89, 89, 0, 1, 0, (byte)(pduData.Length + 22), 1, 2, .. WifiHost.Select(x => (byte)x), 0, 0, 0, 0, 0, 0, 0, (byte)(pduData.Length + 2), .. pduData, .. checkSumData];
        await inverterSocket.SendAsync(responseData, cancellationToken);
    }
}
