using Wyrm.ModBusClient.GivEnergy.Constants;
using Wyrm.ModBusClient.GivEnergy.Extensions;
using Wyrm.ModBusClient.GivEnergy.Responses;
using Wyrm.ModBusClient.GivEnergy.Responses.Models;

namespace Wyrm.ModBusClient.GivEnergy.Services;

internal sealed class InverterDataConverter : IInverterDataConverter
{
    internal const byte InverterId = 0x11;
    internal const byte MeterMin = 0x01;
    internal const byte MeterMax = 0x08;
    internal const byte LowVoltageBCUId = 0x31;
    internal const byte BatteryMin = 0x32;
    internal const byte BatteryMax = 0x37;
    private const byte HoldingRegisters = 0x03;
    private const byte InputRegisters = 0x04;

    public GivEnergyResponse ParseResponse(string serialNo, string wifiHost, int registerAddress, UshortDataResponse response)
    {
        if (response.UshortData.Count != 60)
            throw new GivEnergyClientException("Not enough registers");

        return response.UnitIdentifier switch
        {
            InverterId when response.FunctionNumber is HoldingRegisters && registerAddress == 0 =>
                PopulateInverterProperties1(serialNo, wifiHost, [.. response.UshortData]),

            InverterId when response.FunctionNumber is HoldingRegisters && registerAddress == 60 =>
                PopulateInverterProperties2(serialNo, wifiHost, [.. response.UshortData]),

            InverterId when response.FunctionNumber is HoldingRegisters && registerAddress == 120 =>
                PopulateInverterProperties3(serialNo, wifiHost, [.. response.UshortData]),

            InverterId when response.FunctionNumber is HoldingRegisters && registerAddress == 180 =>
                PopulateInverterProperties4(serialNo, wifiHost, [.. response.UshortData]),

            InverterId when response.FunctionNumber is HoldingRegisters && registerAddress == 240 =>
                PopulateInverterProperties5(serialNo, wifiHost, [.. response.UshortData]),

            InverterId when response.FunctionNumber is HoldingRegisters && registerAddress == 300 =>
                PopulateInverterProperties6(serialNo, wifiHost, [.. response.UshortData]),

            InverterId when response.FunctionNumber is HoldingRegisters && registerAddress == 480 =>
                PopulateInverterProperties9(serialNo, wifiHost, [.. response.UshortData]),

            InverterId when response.FunctionNumber is HoldingRegisters && registerAddress == 540 =>
                PopulateInverterProperties10(serialNo, wifiHost, [.. response.UshortData]),

            InverterId when response.FunctionNumber is InputRegisters && registerAddress == 0 =>
                PopulateInverterData1(serialNo, wifiHost, [.. response.UshortData]),

            InverterId when response.FunctionNumber is InputRegisters && registerAddress == 240 =>
                PopulateInverterData5(serialNo, wifiHost, [.. response.UshortData]),

            >= MeterMin and <= MeterMax when response.FunctionNumber is InputRegisters && registerAddress == 60 =>
                PopulateMeterData2(serialNo, wifiHost, response.UnitIdentifier, [.. response.UshortData]),

            LowVoltageBCUId when response.FunctionNumber is InputRegisters && registerAddress == 60 =>
                PopulateLowVoltageBCUData2(serialNo, wifiHost, [.. response.UshortData]),

            >= BatteryMin and <= BatteryMax when response.FunctionNumber is InputRegisters && registerAddress == 60 =>
                PopulateBattery2(serialNo, wifiHost, (byte)(response.UnitIdentifier - BatteryMin + 1), [.. response.UshortData]),

            _ => PopulateUndecodedData(serialNo, wifiHost, registerAddress, response)
        };
    }

    private static GivEnergyResponse PopulateInverterProperties1(string serialNo, string wifiHost, IList<ushort> data)
    {
        return new GivEnergyResponse
        {
            SerialNumber = serialNo,
            WifiAdapter = wifiHost,
            ResponseDataType = Responses.Constants.ResponseDataType.InverterProperties1,
            ResponseData = new InverterProperties1
            {
                DeviceTypeCode = data[0].ConvertHex(),
                Model = data[0].ConvertModel(),
                Module = data[1].ConvertHex(data[2]),
                NumberOfMPPT = data[3] >> 8,
                NumberOfPhases = data[3] & 0xFF,
                Reg005 = data[4],
                Reg006 = data[5],
                Reg007 = data[6],
                EnableAmmeter = data[7] != 0,
                UnusedSerialNumber = data[8].ConvertString(data[9], data[10], data[11], data[12]),
                SerialNumber = data[13].ConvertString(data[14], data[15], data[16], data[17]),
                FirstBatteryBMSFirmwareVersion = data[18],
                DSPFirmwareVersion = data[19],
                EnableChargeTarget = data[20] != 0,
                ARMFirmwareVersion = data[21],
                UsbDeviceType = (UsbDeviceType)data[22],
                SelectARMChip = data[23] != 0,
                VariableAddress = data[24],
                VariableValue = data[25],
                GridPortMaximumPowerOutput = data[26],
                BatteryPowerMode = (BatteryPowerMode)data[27],
                Enable60HzFrequencyMode = data[28] != 0,
                BatteryCalibrationStage = (BatteryCalibrationStage)data[29],
                ModbusAddress = data[30],
                ChargeSlot2 = data[31].ConvertTimeSlot(data[32]),
                UserCode = data[33],
                ModbusVersion = $"{data[34].ConvertCenti():0.00}",
                SystemTime = data[35].ConvertDateTime(data[36], data[37], data[38], data[39], data[40]),
                EnableDRMRJ45Port = data[41] != 0,
                EnableReversedCTClamp = data[42] != 0,
                ChargeState = data[43] >> 8,
                DischargeState = data[43] & 0xFF,
                DischargeSlot2 = data[44].ConvertTimeSlot(data[45]),
                BMSFirmwareVersion = data[46],
                MeterType = (MeterType)data[47],
                EnableReversed115Meter = data[48] != 0,
                EnableReversed418Meter = data[49] != 0,
                ActivePowerRate = data[50],
                ReactivePowerRate = data[51],
                PowerFactor = data[52].ConvertPowerFactor(),
                EnableInverterAutoRestart = (data[53] >> 8) != 0,
                EnableInverter = (data[53] & 0xFF) != 0,
                BatteryType = (BatteryType)data[54],
                BatteryCapacity = data[55],
                DischargeSlot1 = data[56].ConvertTimeSlot(data[57]),
                EnableAutoJudgeBatteryType = data[58] != 0,
                EnableDischarge = data[59] != 0
            }
        };
    }

    private static GivEnergyResponse PopulateInverterProperties2(string serialNo, string wifiHost, IList<ushort> data)
    {
        return new GivEnergyResponse
        {
            SerialNumber = serialNo,
            WifiAdapter = wifiHost,
            ResponseDataType = Responses.Constants.ResponseDataType.InverterProperties2,
            ResponseData = new InverterProperties2
            {
                PVStartVoltage = data[0].ConvertDeci(),
                StartCountdownTimer = data[1],
                RestartDelayTime = data[2],
                ACLowLimitTripVoltage = data[3].ConvertDeci(),
                ACHighLimitTripVoltage = data[4].ConvertDeci(),
                ACLowLimitTripFrequency = data[5].ConvertCenti(),
                ACHighLimitTripFrequency = data[6].ConvertCenti(),
                ACLowVoltageTripTime = data[7].ConvertCenti(),
                ACHighVoltageTripTime = data[8].ConvertCenti(),
                ACLowFrequencyTripTime = data[9].ConvertCenti(),
                ACHighFrequencyTripTime = data[10].ConvertCenti(),
                ACLowLimitReconnectVoltage = data[11].ConvertDeci(),
                ACHighLimitReconnectVoltage = data[12].ConvertDeci(),
                ACLowLimitReconnectFrequency = data[13].ConvertCenti(),
                ACHighLimitReconnectFrequency = data[14].ConvertCenti(),
                ACLowVoltageReconnectTime = data[15].ConvertCenti(),
                ACHighVoltageReconnectTime = data[16].ConvertCenti(),
                ACLowFrequencyReconnectTime = data[17].ConvertCenti(),
                ACHighFrequencyReconnectTime = data[18].ConvertCenti(),
                ACLowLimitGridVoltage = data[19].ConvertDeci(),
                ACHighLimitGridVoltage = data[20].ConvertDeci(),
                ACLowLimitGridFrequency = data[21].ConvertCenti(),
                ACHighLimitGridFrequency = data[22].ConvertCenti(),
                AC10MinuteProtectVoltage = data[23].ConvertDeci(),
                ISOProtection1 = data[24],
                ISOProtection2 = data[25],
                GFCIProtectionValue1 = data[26],
                GFCIProtectionTime1 = data[27],
                GFCIProtectionValue2 = data[28],
                GFCIProtectionTime2 = data[29],
                DCIProtectionValue1 = data[30],
                DCIProtectionTime1 = data[31],
                DCIProtectionValue2 = data[32],
                DCIProtectionTime2 = data[33],
                ChargeSlot1 = data[34].ConvertTimeSlot(data[35]),
                EnableCharge = data[36] != 0,
                BatteryLowVoltageProtectionLimit = data[37].ConvertCenti(),
                BatteryHighVoltageProtectionLimit = data[38].ConvertCenti(),
                String1VoltageAdjustment = data[39],
                String2VoltageAdjustment = data[40],
                GridImportLimit = data[41],
                GridImportLimitEnabled = data[42] != 0,
                EnableLORA = data[43] != 0,
                EnableBatterySelfHeating = data[44] != 0,
                BatteryVoltageAdjust = data[45].ConvertCenti(),
                String1PowerAdjustment = data[46],
                String2PowerAdjustment = data[47],
                BatteryLowForceChargeTime = data[48],
                EnableBMSRead = data[49] != 0,
                BatteryStateOfChargeReserve = data[50],
                BatteryChargeLimit = data[51],
                BatteryDischargeLimit = data[52],
                EnableBuzzer = data[53] != 0,
                BatteryDischargeMinPowerReserve = data[54],
                Reg116 = data[55],
                ChargeTargetStateOfCharge = data[56],
                ChargeStateOfChargeStop2 = data[57],
                DischargeStateOfChargeStop2 = data[58],
                ChargeStateOfChargeStop1 = data[59]
            }
        };
    }

    private static GivEnergyResponse PopulateInverterProperties3(string serialNo, string wifiHost, IList<ushort> data)
    {
        return new GivEnergyResponse
        {
            SerialNumber = serialNo,
            WifiAdapter = wifiHost,
            ResponseDataType = Responses.Constants.ResponseDataType.InverterProperties3,
            ResponseData = new InverterProperties3
            {
                DischargeStateOfChargeStop1 = data[0],
                EnableLocalCommandTest = data[1] != 0,
                PowerFactorFunction = (PowerFactorFunction)data[2],
                FrequencyLoadLimitRate = data[3],
                EnableLowVoltageFaultRideThrough = data[4] != 0,
                EnableFrequencyDerating = data[5] != 0,
                EnableAbove6kWSystem = data[6] != 0,
                StartSystemAutoTest = data[7] != 0,
                EnableSPI = data[8] != 0,
                PowerFactorCommandMemoryState = data[9],
                PowerFactorPoints =
                [
                    (data[10], data[11]),
                    (data[12], data[13]),
                    (data[14], data[15]),
                    (data[16], data[17])
                ],
                CEI021V1SQuotient = data[18].ConvertDeci(),
                CEI021V2SQuotient = data[19].ConvertDeci(),
                CEI021V1LQuotient = data[20].ConvertDeci(),
                CEI021V2LQuotient = data[21].ConvertDeci(),
                CEI021LockInActivePower = data[22],
                CEI021LockOutActivePower = data[23],
                CEI021LockInGridVoltage = data[24].ConvertDeci(),
                CEI021LockOutGridVoltage = data[25].ConvertDeci(),
                LVFRTReactiveRate = data[26],
                LVFRTLowFaults =
                [
                    (data[27], data[28]),
                    (data[29], data[30]),
                    (data[31], data[32]),
                    (data[33], data[34])
                ],
                LVFRTHighFaults =
                [
                    (data[35], data[36])
                ],
                Reg158 = data[37],
                Reg159 = data[38],
                Reg160 = data[39],
                Reg161 = data[40],
                Reg162 = data[41],
                ResetUserInformation = data[42],
                InverterReboot = data[43],
                Reg165 = data[44],
                Reg166 = data[45],
                EnableRealTimeControl = data[46] != 0,
                ThreePhaseBalanceMode = data[47],
                ThreePhaseABC = data[48],
                ThreePhaseBalance1 = data[49],
                ThreePhaseBalance2 = data[50],
                ThreePhaseBalance3 = data[51],
                Reg173 = data[52],
                Reg174 = data[53],
                Reg175 = data[54],
                EnableBatteryOnPVOrGrid = data[55] != 0,
                DebugInverter = data[56],
                EnableUPSMode = data[57] != 0,
                EnableG100LimitSwitch = data[58] != 0,
                EnableBatteryCableImpedanceAlarm = data[59] != 0
            }
        };
    }

    private static GivEnergyResponse PopulateInverterProperties4(string serialNo, string wifiHost, IList<ushort> data)
    {
        return new GivEnergyResponse
        {
            SerialNumber = serialNo,
            WifiAdapter = wifiHost,
            ResponseDataType = Responses.Constants.ResponseDataType.InverterProperties4,
            ResponseData = new InverterProperties4
            {
                Reg181To199 = [.. data.Take(19)],
                EnableInverterParallelMode = data[19] != 0,
                CommandBMDFlashUpdate = data[20] != 0,
                Reg222 = data[21],
                Reg223 = data[22],
                InverterErrors = data[23].ConvertUint(data[24]),
                InverterFaultCodes = data[23].ConvertInverterFaultCodes(data[24]),
                Reg226To240 = [.. data.Skip(25)]
            }
        };
    }

    private static GivEnergyResponse PopulateInverterProperties5(string serialNo, string wifiHost, IList<ushort> data)
    {
        return new GivEnergyResponse
        {
            SerialNumber = serialNo,
            WifiAdapter = wifiHost,
            ResponseDataType = Responses.Constants.ResponseDataType.InverterProperties5,
            ResponseData = new InverterProperties5
            {
                Reg241 = data[0],
                Reg242 = data[1],
                ChargeTargetStateOfCharge1 = data[2],
                ChargeSlot2X = data[3].ConvertTimeSlot(data[4]),
                ChargeTargetStateOfCharge2 = data[5],
                ChargeSlot3 = data[6].ConvertTimeSlot(data[7]),
                ChargeTargetStateOfCharge3 = data[8],
                ChargeSlot4 = data[9].ConvertTimeSlot(data[10]),
                ChargeTargetStateOfCharge4 = data[11],
                ChargeSlot5 = data[12].ConvertTimeSlot(data[13]),
                ChargeTargetStateOfCharge5 = data[14],
                ChargeSlot6 = data[15].ConvertTimeSlot(data[16]),
                ChargeTargetStateOfCharge6 = data[17],
                ChargeSlot7 = data[18].ConvertTimeSlot(data[19]),
                ChargeTargetStateOfCharge7 = data[20],
                ChargeSlot8 = data[21].ConvertTimeSlot(data[22]),
                ChargeTargetStateOfCharge8 = data[23],
                ChargeSlot9 = data[24].ConvertTimeSlot(data[25]),
                ChargeTargetStateOfCharge9 = data[26],
                ChargeSlot10 = data[27].ConvertTimeSlot(data[28]),
                ChargeTargetStateOfCharge10 = data[29],
                Reg271 = data[30],
                Reg272 = data[31],
                DischargeTargetStateOfCharge1 = data[32],
                Reg274 = data[33],
                Reg275 = data[34],
                DischargeTargetStateOfCharge2 = data[35],
                DischargeSlot3 = data[36].ConvertTimeSlot(data[37]),
                DischargeTargetStateOfCharge3 = data[38],
                DischargeSlot4 = data[39].ConvertTimeSlot(data[40]),
                DischargeTargetStateOfCharge4 = data[41],
                DischargeSlot5 = data[42].ConvertTimeSlot(data[43]),
                DischargeTargetStateOfCharge5 = data[44],
                DischargeSlot6 = data[45].ConvertTimeSlot(data[46]),
                DischargeTargetStateOfCharge6 = data[47],
                DischargeSlot7 = data[48].ConvertTimeSlot(data[49]),
                DischargeTargetStateOfCharge7 = data[50],
                DischargeSlot8 = data[51].ConvertTimeSlot(data[52]),
                DischargeTargetStateOfCharge8 = data[53],
                DischargeSlot9 = data[54].ConvertTimeSlot(data[55]),
                DischargeTargetStateOfCharge9 = data[56],
                DischargeSlot10 = data[57].ConvertTimeSlot(data[58]),
                DischargeTargetStateOfCharge10 = data[59]
            }
        };
    }

    private static GivEnergyResponse PopulateInverterProperties6(string serialNo, string wifiHost, IList<ushort> data)
    {
        return new GivEnergyResponse
        {
            SerialNumber = serialNo,
            WifiAdapter = wifiHost,
            ResponseDataType = Responses.Constants.ResponseDataType.InverterProperties6,
            ResponseData = new InverterProperties6
            {
                EnablePlantMode = data[0] != 0,
                PlantRole = data[1],
                PlantMeters = data[2],
                OverFrequencyLoadDropRecoveryDelay = data[3],
                Reg305 = data[4],
                MPPTOperatingMode = data[5],
                ConnectionLoadingSlope = data[6],
                EPSNominalVoltage = data[7].ConvertDeci(),
                BatteryNominalPower = data[8],
                BatteryNominalCurrent = data[9],
                BatteryMaxChargePercentage = data[10],
                ExportPriority = data[11],
                UnderFrequencyAddLoadDelay = data[12],
                BatteryChargeLimitAC = data[13],
                BatteryDischargeLimitAC = data[14],
                EN50549ZeroCurrentLowerVoltageLimit = data[15].ConvertDeci(),
                EN50549ZeroCurrentUpperVoltageLimit = data[16].ConvertDeci(),
                EnableEPS = data[17] != 0,
                BatteryPauseMode = data[18],
                BatteryPauseSlot1 = data[19].ConvertTimeSlot(data[20]),
                OverFrequencyDeratingStartPoint = data[21].ConvertCenti(),
                EnableTariffPricingBatteryLogic = data[22] != 0,
                ImportPriceBatteryDischargeThreshold = data[23],
                ImportPriceBatteryChargeThreshold = data[24],
                ExportPriceBatteryDischargeThreshold = data[25],
                UnderFrequencyDeratingStartPoint = data[26].ConvertCenti(),
                UnderFrequencyLoadingSlope = data[27],
                OverFrequencyDeratingStopPoint = data[28].ConvertCenti(),
                EnableBMSOCVCalibration = data[29] != 0,
                GatewayPowerOffSetting = data[30],
                ForceOffGrid = data[31] != 0,
                EnableMicroGrid = data[32] != 0,
                EnableEVCharger = data[33] != 0,
                EVChargerImportLimit = data[34],
                EVChargerReconnectionWaitTime = data[35],
                EVChargerStateOfChargeLimit = data[36],
                EnableFan = data[37] != 0,
                FanSpeed = data[38],
                EnableGateway = data[39] != 0,
                BMSCommunicationMode = data[40],
                NPERelayToggle = data[41],
                AFCISetting = data[42],
                EnableGenerator = data[43] != 0,
                GeneratorStartStateOfCharge = data[44],
                GeneratorStopStateOfCharge = data[45],
                GeneratorChargePower = data[46],
                DisableLEDs = data[47] != 0,
                LCDScreenIdleTimeout = data[48],
                LeadAcidBatteryCalibrationUpperLimit = data[49],
                LeadAcidbatteryCalibrationLowerLimit = data[50],
                InverterOperatingMode = data[51],
                Reg353 = data[52],
                Reg354 = data[53],
                Reg355 = data[54],
                Reg356 = data[55],
                Reg357 = data[56],
                Reg358 = data[57],
                Reg359 = data[58],
                Reg360 = data[59]
            }
        };
    }

    private static GivEnergyResponse PopulateInverterProperties9(string serialNo, string wifiHost, IList<ushort> data)
    {
        return new GivEnergyResponse
        {
            SerialNumber = serialNo,
            WifiAdapter = wifiHost,
            ResponseDataType = Responses.Constants.ResponseDataType.InverterProperties9,
            ResponseData = new InverterProperties9
            {
                Reg481To499 = [.. data.Take(19)],
                HVCabinetCount = data[19],
                HVRacksPerCabinet = data[20],
                HVBatteriesPerRack = data[21],
                HVCellsPerBattery = data[22],
                HVTotalCells = data[23],
                HVTemperatureSensorsPerBattery = data[24],
                HVTotalTemperatureSensors = data[25],
                HVMaxPCSPower = data[26],
                HVMaxChargeVoltage = data[27].ConvertDeci(),
                HVMinDischargeVoltage = data[28].ConvertDeci(),
                HVMaxChargeCurrent = data[29],
                HVParallelCount = data[30],
                Reg512To540 = [.. data.Skip(30)]
            }
        };
    }

    private static GivEnergyResponse PopulateInverterProperties10(string serialNo, string wifiHost, IList<ushort> data)
    {
        return new GivEnergyResponse
        {
            SerialNumber = serialNo,
            WifiAdapter = wifiHost,
            ResponseDataType = Responses.Constants.ResponseDataType.InverterProperties10,
            ResponseData = new InverterProperties10
            {
                Reg541To554 = [.. data.Take(14)],
                SmartLoadSlot1 = data[14].ConvertTimeSlot(data[15]),
                SmartLoadSlot2 = data[16].ConvertTimeSlot(data[17]),
                SmartLoadSlot3 = data[18].ConvertTimeSlot(data[19]),
                SmartLoadSlot4 = data[20].ConvertTimeSlot(data[21]),
                SmartLoadSlot5 = data[22].ConvertTimeSlot(data[23]),
                SmartLoadSlot6 = data[24].ConvertTimeSlot(data[25]),
                SmartLoadSlot7 = data[26].ConvertTimeSlot(data[27]),
                SmartLoadSlot8 = data[28].ConvertTimeSlot(data[29]),
                SmartLoadSlot9 = data[30].ConvertTimeSlot(data[31]),
                SmartLoadSlot10 = data[32].ConvertTimeSlot(data[33]),
                Reg575To600 = [.. data.Skip(33)]
            }
        };
    }

    private static GivEnergyResponse PopulateInverterData1(string serialNo, string wifiHost, IList<ushort> data)
    {
        return new GivEnergyResponse
        {
            SerialNumber = serialNo,
            WifiAdapter = wifiHost,
            ResponseDataType = Responses.Constants.ResponseDataType.InverterData1,
            ResponseData = new InverterData1
            {
                Status = (GivEnergyStatus)data[0],
                PV1Voltage = data[1].ConvertDeci(),
                PV2Voltage = data[2].ConvertDeci(),
                PBusVoltage = data[3].ConvertDeci(),
                NBusVoltage = data[4].ConvertDeci(),
                GridVoltage = data[5].ConvertDeci(),
                BatteryThroughput = data[6].ConvertDeci(data[7]),
                PV1InputCurrent = data[8].ConvertCenti(),
                PV2InputCurrent = data[9].ConvertCenti(),
                GridOutputCurrent = data[10].ConvertCenti(),
                PVGeneratingCapacityTotal = data[11].ConvertDeci(data[12]),
                GridFrequency = data[13].ConvertCenti(),
                ChargeStatus = data[14],
                ChargeStatusType = data[14].ConvertChargeStatus(),
                HighbrighBusVoltage = data[15].ConvertDeci(),
                InverterOutputPowerFactorNow = data[16].ConvertPowerFactor(),
                PV1EnergyToday = data[17].ConvertDeci(),
                PV1InputPower = data[18],
                PV2EnergyToday = data[19].ConvertDeci(),
                PV2InputPower = data[20],
                GridOutEnergyTotal = data[21].ConvertDeci(data[22]),
                PVSolarDiverterEnergy = data[23].ConvertDeci(),
                GridPowerPH1 = data[24].ConvertSigned(),
                GridOutEnergyToday = data[25].ConvertDeci(),
                GridInEnergyToday = data[26].ConvertDeci(),
                InverterInEnergyTotal = data[27].ConvertDeci(data[28]),
                DischargeEnergyYear = data[29].ConvertDeci(),
                GridPowerAtMeter = data[30].ConvertSigned(),
                BackupPower = data[31],
                GridInEnergyTotal = data[32].ConvertDeci(data[33]),
                Reg0034 = data[34],
                ACChargeEnergyToday = data[35].ConvertDeci(),
                BatteryChargeEnergyTodayAlt1 = data[36].ConvertDeci(),
                BatteryDischargeEnergyTodayAlt1 = data[37].ConvertDeci(),
                Countdown = data[38],
                InverterFaultCode = data[39].ConvertHex(),
                InverterWarningCode = data[40].ConvertHex(),
                InverterHeatsinkTemperature = data[41].ConvertDeciSigned(),
                LoadPowerDemand = data[42],
                GridPowerApparent = data[43],
                PVGenerationEnergyToday = data[44].ConvertDeci(),
                PVGenerationEnergyTotal = data[45].ConvertDeci(data[46]),
                WorkTimeTotal = data[47].ConvertTimeSpanHours(data[48]),
                SystemMode = data[49],
                BatteryVoltage = data[50].ConvertCenti(),
                BatteryCurrent = data[51].ConvertCentiSigned(),
                BatteryPower = data[52].ConvertSigned(),
                AC1OutputVoltage = data[53].ConvertDeci(),
                AC1OutputFrequency = data[54].ConvertCenti(),
                ChargerTemperature = data[55].ConvertDeciSigned(),
                BatteryTemperature = data[56].ConvertDeciSigned(),
                ChargerWarningCode = data[57],
                ChargerWarningMessages = data[57].ConvertChargerWarningCode(),
                GridPortCurrent = data[58].ConvertCenti(),
                BatteryPercentage = data[59]
            }
        };
    }

    private static GivEnergyResponse PopulateInverterData5(string serialNo, string wifiHost, IList<ushort> data)
    {
        return new GivEnergyResponse
        {
            SerialNumber = serialNo,
            WifiAdapter = wifiHost,
            ResponseDataType = Responses.Constants.ResponseDataType.InverterData5,
            ResponseData = new InverterData5
            {
                Voltage5 = data[0].ConvertDeci(),
                Current5 = data[6].ConvertCenti(),
                CombinedGenerationPower = data[7].ConvertDeci(data[8])
            }
        };
    }

    private static GivEnergyResponse PopulateMeterData2(string serialNo, string wifiHost, byte meterNo, IList<ushort> data)
    {
        return new GivEnergyResponse
        {
            SerialNumber = serialNo,
            WifiAdapter = wifiHost,
            DeviceNumber = meterNo,
            ResponseDataType = Responses.Constants.ResponseDataType.MeterData2,
            ResponseData = new MeterData2
            {
                Phase1 = new MeterPhaseData
                {
                    Voltage = data[0].ConvertDeci(),
                    Current = data[3].ConvertCenti(),
                    ActivePower = data[8].ConvertSigned(),
                    ReactivePower = data[12].ConvertSigned(),
                    ApparentPower = data[16].ConvertDeci(),
                    PowerFactor = data[20].ConvertPowerFactor()
                },
                Phase2 = new MeterPhaseData
                {
                    Voltage = data[1].ConvertDeci(),
                    Current = data[4].ConvertCenti(),
                    ActivePower = data[9].ConvertSigned(),
                    ReactivePower = data[13].ConvertSigned(),
                    ApparentPower = data[17].ConvertDeci(),
                    PowerFactor = data[21].ConvertPowerFactor()
                },
                Phase3 = new MeterPhaseData
                {
                    Voltage = data[2].ConvertDeci(),
                    Current = data[5].ConvertCenti(),
                    ActivePower = data[10].ConvertSigned(),
                    ReactivePower = data[14].ConvertSigned(),
                    ApparentPower = data[18].ConvertDeci(),
                    PowerFactor = data[22].ConvertPowerFactor()
                },
                LineCurrent = data[6].ConvertCenti(),
                TotalCurrent = data[7].ConvertCenti(),
                ActiveTotalPower = data[11].ConvertSigned(),
                ReactiveTotalPower = data[15].ConvertSigned(),
                ApparentTotalPower = data[19].ConvertDeci(),
                TotalPowerFactor = data[23].ConvertPowerFactor(),
                Frequency = data[24].ConvertCenti(),
                ActiveImportEnergy = data[25].ConvertDeci(),
                ReactiveImportEnergy = data[26].ConvertDeci(),
                ActiveExportEnergy = data[27].ConvertDeci(),
                ReactiveExportEnergy = data[28].ConvertDeci()
            }
        };
    }

    private static GivEnergyResponse PopulateLowVoltageBCUData2(string serialNo, string wifiHost, IList<ushort> data)
    {
        return new GivEnergyResponse
        {
            SerialNumber = serialNo,
            WifiAdapter = wifiHost,
            ResponseDataType = Responses.Constants.ResponseDataType.LowVoltageBCUData2,
            ResponseData = new LowVoltageBCUData2
            {
                BMSStatus1 = data[0],
                BMSStatus2 = data[1],
                RequestChargeCurrent = data[2],
                RequestDischargeCurrent = data[3]
            }
        };
    }

    private static GivEnergyResponse PopulateBattery2(string serialNo, string wifiHost, byte batteryNo, IList<ushort> data)
    {
        return new GivEnergyResponse
        {
            SerialNumber = serialNo,
            WifiAdapter = wifiHost,
            DeviceNumber = batteryNo,
            ResponseDataType = Responses.Constants.ResponseDataType.BatteryData2,
            ResponseData = new BatteryData2
            {
                CellVoltages =
                [
                    data[0].ConvertMilli(),
                    data[1].ConvertMilli(),
                    data[2].ConvertMilli(),
                    data[3].ConvertMilli(),
                    data[4].ConvertMilli(),
                    data[5].ConvertMilli(),
                    data[6].ConvertMilli(),
                    data[7].ConvertMilli(),
                    data[8].ConvertMilli(),
                    data[9].ConvertMilli(),
                    data[10].ConvertMilli(),
                    data[11].ConvertMilli(),
                    data[12].ConvertMilli(),
                    data[13].ConvertMilli(),
                    data[14].ConvertMilli(),
                    data[15].ConvertMilli(),
                ],
                CellsTemperature =
                [
                    data[16].ConvertDeciSigned(),
                    data[17].ConvertDeciSigned(),
                    data[18].ConvertDeciSigned(),
                    data[19].ConvertDeciSigned()
                ],
                CellVoltageSum = data[20].ConvertDeci(),
                BMSMosfetTemperature = data[21].ConvertDeciSigned(),
                OutVoltage = data[22].ConvertMilli(data[23]),
                CalibratedCapacity = data[24].ConvertCenti(data[25]),
                DesignCapacity = data[26].ConvertCenti(data[27]),
                RemainingCapacity = data[28].ConvertCenti(data[29]),
                Status1 = (byte)(data[30] >> 8),
                Status2 = (byte)(data[30] & 0xFF),
                Status3 = (byte)(data[31] >> 8),
                Status4 = (byte)(data[31] & 0xFF),
                Status5 = (byte)(data[32] >> 8),
                Status6 = (byte)(data[32] & 0xFF),
                Status7 = (byte)(data[33] >> 8),
                Warning1 = (byte)(data[34] >> 8),
                Warning2 = (byte)(data[34] & 0xFF),
                Current = data[35].ConvertCentiSigned(),
                NumberOfCycles = data[36],
                NumberofCells = data[37],
                BMSFirmwareVersion = data[38],
                Reg160 = data[39],
                StateOfCharge = data[40],
                DesignCapacity2 = data[41].ConvertCenti(data[42]),
                MaximumTemperature = data[43].ConvertDeciSigned(),
                MinimumTemperature = data[44].ConvertDeciSigned(),
                DischargeTotalEnergy = data[45].ConvertDeci(),
                ChargeTotalEnergy = data[46].ConvertDeci(),
                ForceDischargeFlag = data[47],
                Reg169 = data[48],
                Reg170 = data[49],
                SerialNumber = data[50].ConvertString(data[51], data[52], data[53], data[54]),
                UsbDevice = data[55],
                Reg177 = data[56],
                Reg178 = data[57],
                Reg179 = data[58],
                Reg180 = data[59]
            }
        };
    }

    private static GivEnergyResponse PopulateUndecodedData(string serialNo, string wifiHost, int registerAddress, UshortDataResponse response)
    {
        return new GivEnergyResponse
        {
            SerialNumber = serialNo,
            WifiAdapter = wifiHost,
            ResponseDataType = Responses.Constants.ResponseDataType.RegisterData,
            ResponseData = new RegisterData
            {
                UnitIdentifier = response.UnitIdentifier,
                FunctionNumber = response.FunctionNumber,
                StartAddress = registerAddress,
                RegisterValues = [.. response.UshortData]
            }
        };
    }
}
