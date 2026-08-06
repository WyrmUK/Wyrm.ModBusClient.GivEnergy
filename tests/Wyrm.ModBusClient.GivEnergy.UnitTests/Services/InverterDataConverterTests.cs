using Shouldly;
using Wyrm.ModBusClient.GivEnergy.Constants;
using Wyrm.ModBusClient.GivEnergy.Responses;
using Wyrm.ModBusClient.GivEnergy.Responses.Constants;
using Wyrm.ModBusClient.GivEnergy.Responses.Models;
using Wyrm.ModBusClient.GivEnergy.Services;

namespace Wyrm.ModBusClient.GivEnergy.UnitTests.Services;

public class InverterDataConverterTests
{
    #region Setup

    private readonly IInverterDataConverter _inverterDataConverter = new InverterDataConverter();

    private const string SerialNo = "ID12345678";
    private const string WifiHost = "WH12345678";
    private const ushort TransactionId = 0x5959;
    private const byte HoldingRegisters = 0x03;
    private const byte InputRegisters = 0x04;

    #endregion

    [Theory, CombinatorialData]
    public void ParseResponse_Should_Throw_GivEnergyClientException_If_Not_60_Registers([CombinatorialValues(0, 1, 59, 61)] int numRegisters)
    {
        var response = new UshortDataResponse
        {
            TransactionId = TransactionId,
            UnitIdentifier = InverterDataConverter.InverterId,
            FunctionNumber = HoldingRegisters,
            UshortData = new ushort[numRegisters]
        };
        Should.Throw<GivEnergyClientException>(() => _inverterDataConverter.ParseResponse(SerialNo, WifiHost, 0, response));
    }

    private static readonly ushort[] ZeroData = new ushort[60];

    private static readonly GivEnergyResponse InverterProperties1ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties1,
        ResponseData = new InverterProperties1
        {
            DeviceTypeCode = "0000",
            Module = "00000000",
            ModbusVersion = "0.00",
            PowerFactor = -1M
        }
    };

    private static readonly GivEnergyResponse InverterProperties2ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties2,
        ResponseData = new InverterProperties2()
    };

    private static readonly GivEnergyResponse InverterProperties3ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties3,
        ResponseData = new InverterProperties3
        {
            PowerFactorPoints = new (ushort, ushort)[4],
            LVFRTLowFaults = new (ushort, ushort)[4],
            LVFRTHighFaults = new (ushort, ushort)[1]
        }
    };

    private static readonly GivEnergyResponse InverterProperties4ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties4,
        ResponseData = new InverterProperties4
        {
            Reg181To199 = new ushort[19],
            Reg206To240 = new ushort[35]
        }
    };

    private static readonly GivEnergyResponse InverterProperties5ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties5,
        ResponseData = new InverterProperties5()
    };

    private static readonly GivEnergyResponse InverterProperties6ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties6,
        ResponseData = new InverterProperties6()
    };

    private static readonly GivEnergyResponse InverterProperties9ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties9,
        ResponseData = new InverterProperties9
        {
            Reg481To499 = new ushort[19],
            Reg512To540 = new ushort[29]
        }
    };

    private static readonly GivEnergyResponse InverterProperties10ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties10,
        ResponseData = new InverterProperties10
        {
            Reg541To554 = new ushort[14],
            Reg575To600 = new ushort[26]
        }
    };

    private static readonly GivEnergyResponse InverterData1ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterData1,
        ResponseData = new InverterData1
        {
            InverterOutputPowerFactorNow = -1M
        }
    };

    private static readonly GivEnergyResponse InverterData5ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterData5,
        ResponseData = new InverterData5()
    };

    private static GivEnergyResponse MeterData2ZeroResponse(byte deviceNumber) => new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = deviceNumber,
        ResponseDataType = ResponseDataType.MeterData2,
        ResponseData = new MeterData2
        {
            Phase1 = new MeterPhaseData { PowerFactor = -1M },
            Phase2 = new MeterPhaseData { PowerFactor = -1M },
            Phase3 = new MeterPhaseData { PowerFactor = -1M },
            TotalPowerFactor = -1M
        }
    };

    private static readonly GivEnergyResponse LowVoltageBCUData2ZeroResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.LowVoltageBCUData2,
        ResponseData = new LowVoltageBCUData2()
    };

    private static GivEnergyResponse BatteryData2ZeroResponse(byte deviceNumber) => new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = deviceNumber,
        ResponseDataType = ResponseDataType.BatteryData2,
        ResponseData = new BatteryData2
        {
            CellVoltages = new decimal[16],
            CellsTemperature = new decimal[4]
        }
    };

    private static readonly ushort[] UniqueData = [.. Enumerable.Range(0x2000, 60).Select(x => (ushort)x)];

    private static ushort MakeGoodTimeOnly(ushort data) =>
        (ushort)((data / 100) % 24 * 100 + data % 60);

    private static ushort[] ForInverterProperties1(ushort[] uniqueData)
    {
        ushort[] data = [.. uniqueData];
        var index = 8;
        for (; index < 18; ++index)
        {
            data[index] = (ushort)(('A' << 8) + 'A' + index);
            if (index < 14)
            {
                data[27 + index] = (ushort)index;
            }
        }
        data[31] = MakeGoodTimeOnly(data[31]);
        data[32] = MakeGoodTimeOnly(data[32]);
        data[44] = MakeGoodTimeOnly(data[44]);
        data[45] = MakeGoodTimeOnly(data[45]);
        data[56] = MakeGoodTimeOnly(data[56]);
        data[57] = MakeGoodTimeOnly(data[57]);
        return data;
    }

    private static readonly GivEnergyResponse InverterProperties1UniqueResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties1,
        ResponseData = new InverterProperties1
        {
            DeviceTypeCode = "2000",
            Model = InverterModel.Hybrid,
            Module = "20012002",
            NumberOfMPPT = 32,
            NumberOfPhases = 3,
            Reg005 = 8196,
            Reg006 = 8197,
            Reg007 = 8198,
            EnableAmmeter = true,
            UnusedSerialNumber = "AIAJAKALAM",
            SerialNumber = "ANAOAPAQAR",
            FirstBatteryBMSFirmwareVersion = 8210,
            DSPFirmwareVersion = 8211,
            EnableChargeTarget = true,
            ARMFirmwareVersion = 8213,
            UsbDeviceType = (UsbDeviceType)8214,
            SelectARMChip = true,
            VariableAddress = 8216,
            VariableValue = 8217,
            GridPortMaximumPowerOutput = 8218,
            BatteryPowerMode = (BatteryPowerMode)8219,
            Enable60HzFrequencyMode = true,
            BatteryCalibrationStage = (BatteryCalibrationStage)8221,
            ModbusAddress = 8222,
            ChargeSlot2 = (new TimeOnly(10, 03), new TimeOnly(10, 04)),
            UserCode = 8225,
            ModbusVersion = "82.26",
            SystemTime = new DateTime(2008, 9, 10, 11, 12, 13),
            EnableDRMRJ45Port = true,
            EnableReversedCTClamp = true,
            ChargeState = 32,
            DischargeState = 43,
            DischargeSlot2 = (new TimeOnly(10, 16), new TimeOnly(10, 17)),
            BMSFirmwareVersion = 8238,
            MeterType = (MeterType)8239,
            EnableReversed115Meter = true,
            EnableReversed418Meter = true,
            ActivePowerRate = 8242,
            ReactivePowerRate = 8243,
            PowerFactor = -0.1756M,
            EnableInverterAutoRestart = true,
            EnableInverter = true,
            BatteryType = (BatteryType)8246,
            BatteryCapacity = 8247,
            DischargeSlot1 = (new TimeOnly(10, 28), new TimeOnly(10, 29)),
            EnableAutoJudgeBatteryType = true,
            EnableDischarge = true
        }
    };

    private static ushort[] ForInverterProperties2(ushort[] uniqueData)
    {
        ushort[] data = [.. uniqueData];
        data[34] = MakeGoodTimeOnly(data[34]);
        data[35] = MakeGoodTimeOnly(data[35]);
        return data;
    }

    private static readonly GivEnergyResponse InverterProperties2UniqueResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties2,
        ResponseData = new InverterProperties2
        {
            PVStartVoltage = 819.2M,
            StartCountdownTimer = 8193,
            RestartDelayTime = 8194,
            ACLowLimitTripVoltage = 819.5M,
            ACHighLimitTripVoltage = 819.6M,
            ACLowLimitTripFrequency = 81.97M,
            ACHighLimitTripFrequency = 81.98M,
            ACLowVoltageTripTime = 81.99M,
            ACHighVoltageTripTime = 82.00M,
            ACLowFrequencyTripTime = 82.01M,
            ACHighFrequencyTripTime = 82.02M,
            ACLowLimitReconnectVoltage = 820.3M,
            ACHighLimitReconnectVoltage = 820.4M,
            ACLowLimitReconnectFrequency = 82.05M,
            ACHighLimitReconnectFrequency = 82.06M,
            ACLowVoltageReconnectTime = 82.07M,
            ACHighVoltageReconnectTime = 82.08M,
            ACLowFrequencyReconnectTime = 82.09M,
            ACHighFrequencyReconnectTime = 82.10M,
            ACLowLimitGridVoltage = 821.1M,
            ACHighLimitGridVoltage = 821.2M,
            ACLowLimitGridFrequency = 82.13M,
            ACHighLimitGridFrequency = 82.14M,
            AC10MinuteProtectVoltage = 821.5M,
            ISOProtection1 = 8216,
            ISOProtection2 = 8217,
            GFCIProtectionValue1 = 8218,
            GFCIProtectionTime1 = 8219,
            GFCIProtectionValue2 = 8220,
            GFCIProtectionTime2 = 8221,
            DCIProtectionValue1 = 8222,
            DCIProtectionTime1 = 8223,
            DCIProtectionValue2 = 8224,
            DCIProtectionTime2 = 8225,
            ChargeSlot1 = (new TimeOnly(10, 6), new TimeOnly(10, 7)),
            EnableCharge = true,
            BatteryLowVoltageProtectionLimit = 82.29M,
            BatteryHighVoltageProtectionLimit = 82.30M,
            String1VoltageAdjustment = 8231,
            String2VoltageAdjustment = 8232,
            GridImportLimit = 8233,
            GridImportLimitEnabled = true,
            EnableLORA = true,
            EnableBatterySelfHeating = true,
            BatteryVoltageAdjust = 82.37M,
            String1PowerAdjustment = 8238,
            String2PowerAdjustment = 8239,
            BatteryLowForceChargeTime = 8240,
            EnableBMSRead = true,
            BatteryStateOfChargeReserve = 8242,
            BatteryChargeLimit = 8243,
            BatteryDischargeLimit = 8244,
            EnableBuzzer = true,
            BatteryDischargeMinPowerReserve = 8246,
            Reg116 = 8247,
            ChargeTargetStateOfCharge = 8248,
            ChargeStateOfChargeStop2 = 8249,
            DischargeStateOfChargeStop2 = 8250,
            ChargeStateOfChargeStop1 = 8251
        }
    };

    private static readonly GivEnergyResponse InverterProperties3UniqueResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties3,
        ResponseData = new InverterProperties3
        {
            DischargeStateOfChargeStop1 = 8192,
            EnableLocalCommandTest = true,
            PowerFactorFunction = (PowerFactorFunction)8194,
            FrequencyLoadLimitRate = 8195,
            EnableLowVoltageFaultRideThrough = true,
            EnableFrequencyDerating = true,
            EnableAbove6kWSystem = true,
            StartSystemAutoTest = true,
            EnableSPI = true,
            PowerFactorCommandMemoryState = 8201,
            PowerFactorPoints = [(8202, 8203), (8204, 8205), (8206, 8207), (8208, 8209)],
            CEI021V1SQuotient = 821.0M,
            CEI021V2SQuotient = 821.1M,
            CEI021V1LQuotient = 821.2M,
            CEI021V2LQuotient = 821.3M,
            CEI021LockInActivePower = 8214,
            CEI021LockOutActivePower = 8215,
            CEI021LockInGridVoltage = 821.6M,
            CEI021LockOutGridVoltage = 821.7M,
            LVFRTReactiveRate = 8218,
            LVFRTLowFaults = [(8219, 8220), (8221, 8222), (8223, 8224), (8225, 8226)],
            LVFRTHighFaults = [(8227, 8228)],
            Reg158 = 8229,
            Reg159 = 8230,
            Reg160 = 8231,
            Reg161 = 8232,
            Reg162 = 8233,
            ResetUserInformation = 8234,
            InverterReboot = 8235,
            Reg165 = 8236,
            Reg166 = 8237,
            EnableRealTimeControl = true,
            ThreePhaseBalanceMode = 8239,
            ThreePhaseABC = 8240,
            ThreePhaseBalance1 = 8241,
            ThreePhaseBalance2 = 8242,
            ThreePhaseBalance3 = 8243,
            Reg173 = 8244,
            Reg174 = 8245,
            Reg175 = 8246,
            EnableBatteryOnPVOrGrid = true,
            DebugInverter = 8248,
            EnableUPSMode = true,
            EnableG100LimitSwitch = true,
            EnableBatteryCableImpedanceAlarm = true
        }
    };

    private static readonly GivEnergyResponse InverterProperties4UniqueResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties4,
        ResponseData = new InverterProperties4
        {
            Reg181To199 = [.. UniqueData.Take(19)],
            EnableInverterParallelMode = true,
            CommandBMDFlashUpdate = true,
            Reg202 = 8213,
            Reg203 = 8214,
            InverterErrors = 538_386_456,
            InverterFaultCodes = [ InverterFaultCode.BackupOverloadFault, InverterFaultCode.InverterFrequencyFault, InverterFaultCode.RelayFault, InverterFaultCode.InverterVoltageFault, InverterFaultCode.HallSensorFault, InverterFaultCode.GridVoltageFault],
            Reg206To240 = [.. UniqueData.Skip(25).Take(35)]
        }
    };

    private static ushort[] ForInverterProperties5(ushort[] uniqueData)
    {
        ushort[] data = [.. uniqueData];
        var index = 3;
        for (; index < 29; index += 3)
        {
            data[index] = MakeGoodTimeOnly(data[index]);
            data[index + 1] = MakeGoodTimeOnly(data[index + 1]);
        }
        index = 36;
        for (; index < 59; index += 3)
        {
            data[index] = MakeGoodTimeOnly(data[index]);
            data[index + 1] = MakeGoodTimeOnly(data[index + 1]);
        }
        return data;
    }

    private static readonly GivEnergyResponse InverterProperties5UniqueResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties5,
        ResponseData = new InverterProperties5
        {
            Reg241 = 8192,
            Reg242 = 8193,
            ChargeTargetStateOfCharge1 = 8194,
            ChargeSlot2X = (new TimeOnly(9, 35), new TimeOnly(9, 36)),
            ChargeTargetStateOfCharge2 = 8197,
            ChargeSlot3 = (new TimeOnly(9, 38), new TimeOnly(9, 39)),
            ChargeTargetStateOfCharge3 = 8200,
            ChargeSlot4 = (new TimeOnly(10, 41), new TimeOnly(10, 42)),
            ChargeTargetStateOfCharge4 = 8203,
            ChargeSlot5 = (new TimeOnly(10, 44), new TimeOnly(10, 45)),
            ChargeTargetStateOfCharge5 = 8206,
            ChargeSlot6 = (new TimeOnly(10, 47), new TimeOnly(10, 48)),
            ChargeTargetStateOfCharge6 = 8209,
            ChargeSlot7 = (new TimeOnly(10, 50), new TimeOnly(10, 51)),
            ChargeTargetStateOfCharge7 = 8212,
            ChargeSlot8 = (new TimeOnly(10, 53), new TimeOnly(10, 54)),
            ChargeTargetStateOfCharge8 = 8215,
            ChargeSlot9 = (new TimeOnly(10, 56), new TimeOnly(10, 57)),
            ChargeTargetStateOfCharge9 = 8218,
            ChargeSlot10 = (new TimeOnly(10, 59), new TimeOnly(10, 0)),
            ChargeTargetStateOfCharge10 = 8221,
            Reg271 = 8222,
            Reg272 = 8223,
            DischargeTargetStateOfCharge1 = 8224,
            Reg274 = 8225,
            Reg275 = 8226,
            DischargeTargetStateOfCharge2 = 8227,
            DischargeSlot3 = (new TimeOnly(10, 8), new TimeOnly(10, 9)),
            DischargeTargetStateOfCharge3 = 8230,
            DischargeSlot4 = (new TimeOnly(10, 11), new TimeOnly(10, 12)),
            DischargeTargetStateOfCharge4 = 8233,
            DischargeSlot5 = (new TimeOnly(10, 14), new TimeOnly(10, 15)),
            DischargeTargetStateOfCharge5 = 8236,
            DischargeSlot6 = (new TimeOnly(10, 17), new TimeOnly(10, 18)),
            DischargeTargetStateOfCharge6 = 8239,
            DischargeSlot7 = (new TimeOnly(10, 20), new TimeOnly(10, 21)),
            DischargeTargetStateOfCharge7 = 8242,
            DischargeSlot8 = (new TimeOnly(10, 23), new TimeOnly(10, 24)),
            DischargeTargetStateOfCharge8 = 8245,
            DischargeSlot9 = (new TimeOnly(10, 26), new TimeOnly(10, 27)),
            DischargeTargetStateOfCharge9 = 8248,
            DischargeSlot10 = (new TimeOnly(10, 29), new TimeOnly(10, 30)),
            DischargeTargetStateOfCharge10 = 8251
        }
    };

    private static ushort[] ForInverterProperties6(ushort[] uniqueData)
    {
        ushort[] data = [.. uniqueData];
        data[19] = MakeGoodTimeOnly(data[19]);
        data[20] = MakeGoodTimeOnly(data[20]);
        return data;
    }

    private static readonly GivEnergyResponse InverterProperties6UniqueResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties6,
        ResponseData = new InverterProperties6
        {
            EnablePlantMode = true,
            PlantRole = 8193,
            PlantMeters = 8194,
            OverFrequencyLoadDropRecoveryDelay = 8195,
            Reg305 = 8196,
            MPPTOperatingMode = 8197,
            ConnectionLoadingSlope = 8198,
            EPSNominalVoltage = 819.9M,
            BatteryNominalPower = 8200,
            BatteryNominalCurrent = 8201,
            BatteryMaxChargePercentage = 8202,
            ExportPriority = 8203,
            UnderFrequencyAddLoadDelay = 8204,
            BatteryChargeLimitAC = 8205,
            BatteryDischargeLimitAC = 8206,
            EN50549ZeroCurrentLowerVoltageLimit = 820.7M,
            EN50549ZeroCurrentUpperVoltageLimit = 820.8M,
            EnableEPS = true,
            BatteryPauseMode = 8210,
            BatteryPauseSlot1 = (new TimeOnly(10, 51), new TimeOnly(10, 52)),
            OverFrequencyDeratingStartPoint = 82.13M,
            EnableTariffPricingBatteryLogic = true,
            ImportPriceBatteryDischargeThreshold = 8215,
            ImportPriceBatteryChargeThreshold = 8216,
            ExportPriceBatteryDischargeThreshold = 8217,
            UnderFrequencyDeratingStartPoint = 82.18M,
            UnderFrequencyLoadingSlope = 8219,
            OverFrequencyDeratingStopPoint = 82.20M,
            EnableBMSOCVCalibration = true,
            GatewayPowerOffSetting = 8222,
            ForceOffGrid = true,
            EnableMicroGrid = true,
            EnableEVCharger = true,
            EVChargerImportLimit = 8226,
            EVChargerReconnectionWaitTime = 8227,
            EVChargerStateOfChargeLimit = 8228,
            EnableFan = true,
            FanSpeed = 8230,
            EnableGateway = true,
            BMSCommunicationMode = 8232,
            NPERelayToggle = 8233,
            AFCISetting = 8234,
            EnableGenerator = true,
            GeneratorStartStateOfCharge = 8236,
            GeneratorStopStateOfCharge = 8237,
            GeneratorChargePower = 8238,
            DisableLEDs = true,
            LCDScreenIdleTimeout = 8240,
            LeadAcidBatteryCalibrationUpperLimit = 8241,
            LeadAcidbatteryCalibrationLowerLimit = 8242,
            InverterOperatingMode = 8243,
            Reg353 = 8244,
            Reg354 = 8245,
            Reg355 = 8246,
            Reg356 = 8247,
            Reg357 = 8248,
            Reg358 = 8249,
            Reg359 = 8250,
            Reg360 = 8251
        }
    };

    private static readonly GivEnergyResponse InverterProperties9UniqueResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties9,
        ResponseData = new InverterProperties9
        {
            Reg481To499 = [.. UniqueData.Take(19)],
            HVCabinetCount = 8211,
            HVRacksPerCabinet = 8212,
            HVBatteriesPerRack = 8213,
            HVCellsPerBattery = 8214,
            HVTotalCells = 8215,
            HVTemperatureSensorsPerBattery = 8216,
            HVTotalTemperatureSensors = 8217,
            HVMaxPCSPower = 8218,
            HVMaxChargeVoltage = 821.9M,
            HVMinDischargeVoltage = 822.0M,
            HVMaxChargeCurrent = 8221,
            HVParallelCount = 8222,
            Reg512To540 = [.. UniqueData.Skip(31).Take(29)]
        }
    };

    private static ushort[] ForInverterProperties10(ushort[] uniqueData)
    {
        ushort[] data = [.. uniqueData];
        for (var index = 14; index < 34; index += 2)
        {
            data[index] = MakeGoodTimeOnly(data[index]);
            data[index + 1] = MakeGoodTimeOnly(data[index + 1]);
        }
        return data;
    }

    private static readonly GivEnergyResponse InverterProperties10UniqueResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterProperties10,
        ResponseData = new InverterProperties10
        {
            Reg541To554 = [.. UniqueData.Take(14)],
            SmartLoadSlot1 = (new TimeOnly(10, 46), new TimeOnly(10, 47)),
            SmartLoadSlot2 = (new TimeOnly(10, 48), new TimeOnly(10, 49)),
            SmartLoadSlot3 = (new TimeOnly(10, 50), new TimeOnly(10, 51)),
            SmartLoadSlot4 = (new TimeOnly(10, 52), new TimeOnly(10, 53)),
            SmartLoadSlot5 = (new TimeOnly(10, 54), new TimeOnly(10, 55)),
            SmartLoadSlot6 = (new TimeOnly(10, 56), new TimeOnly(10, 57)),
            SmartLoadSlot7 = (new TimeOnly(10, 58), new TimeOnly(10, 59)),
            SmartLoadSlot8 = (new TimeOnly(10, 0), new TimeOnly(10, 1)),
            SmartLoadSlot9 = (new TimeOnly(10, 2), new TimeOnly(10, 3)),
            SmartLoadSlot10 = (new TimeOnly(10, 4), new TimeOnly(10, 5)),
            Reg575To600 = [.. UniqueData.Skip(34).Take(26)]
        }
    };

    private static readonly GivEnergyResponse InverterData1UniqueResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterData1,
        ResponseData = new InverterData1
        {
            Status = (GivEnergyStatus)8192,
            PV1Voltage = 819.3M,
            PV2Voltage = 819.4M,
            PBusVoltage = 819.5M,
            NBusVoltage = 819.6M,
            GridVoltage = 819.7M,
            BatteryThroughput = 53_727_232.7M,
            PV1InputCurrent = 82.00M,
            PV2InputCurrent = 82.01M,
            GridOutputCurrent = 82.02M,
            PVGeneratingCapacityTotal = 53_760_001.2M,
            GridFrequency = 82.05M,
            ChargeStatus = 8206,
            ChargeStatusType = ChargeStatus.Unknown,
            HighbrighBusVoltage = 820.7M,
            InverterOutputPowerFactorNow = -0.1792M,
            PV1EnergyToday = 820.9M,
            PV1InputPower = 8210,
            PV2EnergyToday = 821.1M,
            PV2InputPower = 8212,
            GridOutEnergyTotal = 53_825_538.2M,
            PVSolarDiverterEnergy = 821.5M,
            GridPowerPH1 = 8216M,
            GridOutEnergyToday = 821.7M,
            GridInEnergyToday = 821.8M,
            InverterInEnergyTotal = 53_864_860.4M,
            DischargeEnergyYear = 822.1M,
            GridPowerAtMeter = 8222M,
            BackupPower = 8223,
            GridInEnergyTotal = 53_897_628.9M,
            Reg0034 = 8226,
            ACChargeEnergyToday = 822.7M,
            BatteryChargeEnergyTodayAlt1 = 822.8M,
            BatteryDischargeEnergyTodayAlt1 = 822.9M,
            Countdown = 8230,
            InverterFaultCode = "2027",
            InverterWarningCode = "2028",
            InverterHeatsinkTemperature = 823.3M,
            LoadPowerDemand = 8234,
            GridPowerApparent = 8235,
            PVGenerationEnergyToday = 823.6M,
            PVGenerationEnergyTotal = 53_982_827.0M,
            WorkTimeTotal = TimeSpan.MaxValue,
            SystemMode = 8241,
            BatteryVoltage = 82.42M,
            BatteryCurrent = 82.43M,
            BatteryPower = 8244M,
            AC1OutputVoltage = 824.5M,
            AC1OutputFrequency = 82.46M,
            ChargerTemperature = 824.7M,
            BatteryTemperature = 824.8M,
            ChargerWarningCode = 8249,
            ChargerWarningMessages = [ChargerWarningCode.BMSUnderTemperatureCharge, ChargerWarningCode.BMSOverTemperatureDischarge, ChargerWarningCode.BMSUnderVoltage, ChargerWarningCode.BMSOverVoltage, ChargerWarningCode.BatteryVoltageLow],
            GridPortCurrent = 82.50M,
            BatteryPercentage = 8251
        }
    };

    private static readonly GivEnergyResponse InverterData5UniqueResponse = new()
    {
        SerialNumber = SerialNo,
        WifiAdapter = WifiHost,
        DeviceNumber = 1,
        ResponseDataType = ResponseDataType.InverterData5,
        ResponseData = new InverterData5
        {
            Voltage5 = 819.2M,
            Current5 = 81.98M,
            CombinedGenerationPower = 53_733_786.4M
        }
    };

    public static TheoryData<byte, byte, ushort[], int, GivEnergyResponse> ParseTests()
    {
        var data = new TheoryData<byte, byte, ushort[], int, GivEnergyResponse>
        {
            { InverterDataConverter.InverterId, HoldingRegisters, ZeroData, 0, InverterProperties1ZeroResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ForInverterProperties1(UniqueData), 0, InverterProperties1UniqueResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ZeroData, 60, InverterProperties2ZeroResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ForInverterProperties2(UniqueData), 60, InverterProperties2UniqueResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ZeroData, 120, InverterProperties3ZeroResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, UniqueData, 120, InverterProperties3UniqueResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ZeroData, 180, InverterProperties4ZeroResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, UniqueData, 180, InverterProperties4UniqueResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ZeroData, 240, InverterProperties5ZeroResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ForInverterProperties5(UniqueData), 240, InverterProperties5UniqueResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ZeroData, 300, InverterProperties6ZeroResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ForInverterProperties6(UniqueData), 300, InverterProperties6UniqueResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ZeroData, 480, InverterProperties9ZeroResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, UniqueData, 480, InverterProperties9UniqueResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ZeroData, 540, InverterProperties10ZeroResponse },
            { InverterDataConverter.InverterId, HoldingRegisters, ForInverterProperties10(UniqueData), 540, InverterProperties10UniqueResponse },
            { InverterDataConverter.InverterId, InputRegisters, ZeroData, 0, InverterData1ZeroResponse },
            { InverterDataConverter.InverterId, InputRegisters, UniqueData, 0, InverterData1UniqueResponse },
            { InverterDataConverter.InverterId, InputRegisters, ZeroData, 240, InverterData5ZeroResponse },
            { InverterDataConverter.InverterId, InputRegisters, UniqueData, 240, InverterData5UniqueResponse }
        };
        for (byte meterNum = 0; meterNum < InverterDataConverter.MeterMax - InverterDataConverter.MeterMin; ++meterNum)
        {
            data.Add((byte)(InverterDataConverter.MeterMin + meterNum), InputRegisters, ZeroData, 60, MeterData2ZeroResponse((byte)(meterNum + 1)));
        }
        data.Add(InverterDataConverter.LowVoltageBCUId, InputRegisters, ZeroData, 60, LowVoltageBCUData2ZeroResponse);
        for (byte batteryNum = 0; batteryNum < InverterDataConverter.BatteryMax - InverterDataConverter.BatteryMin; ++batteryNum)
        {
            data.Add((byte)(InverterDataConverter.BatteryMin + batteryNum), InputRegisters, ZeroData, 60, BatteryData2ZeroResponse((byte)(batteryNum + 1)));
        }
        return data;
    }

    [Theory, MemberData(nameof(ParseTests))]
    public void ParseResponse_Should_Parse_Data(byte unitIdentifier, byte functionNumber, ushort[] data, int address, GivEnergyResponse expected)
    {
        var response = new UshortDataResponse
        {
            TransactionId = TransactionId,
            UnitIdentifier = unitIdentifier,
            FunctionNumber = functionNumber,
            UshortData = data
        };
        var result = _inverterDataConverter.ParseResponse(SerialNo, WifiHost, address, response);
        result.SerialNumber.ShouldBe(expected.SerialNumber);
        result.WifiAdapter.ShouldBe(expected.WifiAdapter);
        result.DeviceNumber.ShouldBe(expected.DeviceNumber);
        result.ResponseDataType.ShouldBe(expected.ResponseDataType);
        result.ResponseData.ShouldBeEquivalentTo(expected.ResponseData);
    }

    private static readonly RegisterData RegisterDataResponse = new()
    {
        UnitIdentifier = 0x30,
        FunctionNumber = InputRegisters,
        StartAddress = 60,
        RegisterValues = [.. UniqueData]
    };

    [Fact]
    public void ParseResponse_Should_Parse_RegisterData()
    {
        var response = new UshortDataResponse
        {
            TransactionId = TransactionId,
            UnitIdentifier = RegisterDataResponse.UnitIdentifier,
            FunctionNumber = RegisterDataResponse.FunctionNumber,
            UshortData = UniqueData
        };
        var result = _inverterDataConverter.ParseResponse(SerialNo, WifiHost, RegisterDataResponse.StartAddress, response);
        result.SerialNumber.ShouldBe(SerialNo);
        result.WifiAdapter.ShouldBe(WifiHost);
        result.DeviceNumber.ShouldBe((byte)0);
        result.ResponseDataType.ShouldBe(ResponseDataType.RegisterData);
        ((RegisterData)result.ResponseData).UnitIdentifier.ShouldBe(RegisterDataResponse.UnitIdentifier);
        ((RegisterData)result.ResponseData).FunctionNumber.ShouldBe(RegisterDataResponse.FunctionNumber);
        ((RegisterData)result.ResponseData).StartAddress.ShouldBe(RegisterDataResponse.StartAddress);
        ((RegisterData)result.ResponseData).RegisterValues.ToArray().ShouldBeEquivalentTo(RegisterDataResponse.RegisterValues.ToArray());
    }
}
