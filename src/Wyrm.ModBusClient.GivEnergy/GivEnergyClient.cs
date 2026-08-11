using Microsoft.Extensions.Logging;
using System.Net;
using Wyrm.ModBusClient.GivEnergy.Extensions;
using Wyrm.ModBusClient.GivEnergy.Responses.Constants;
using Wyrm.ModBusClient.GivEnergy.Services;

namespace Wyrm.ModBusClient.GivEnergy;

internal sealed class GivEnergyClient(
    IModBusRegisterClient _modBusClient,
    IInverterDataConverter _inverterDataConverter,
    IFramerService _framerService,
    ILogger<GivEnergyClient> _logger) : IGivEnergyClient
{
    private const ushort ProtocolIdentifier = 0x0001;
    private const ushort TransactionId = 0x5959;
    private const ushort RegisterBlockCount = 60;

    public async ValueTask ConnectAsync(EndPoint endPoint, CancellationToken cancellationToken = default)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("GivEnergy Client: Connecting to end point: {Address}", endPoint.Serialize().ToString());

        _modBusClient.ProtocolIdentifier = ProtocolIdentifier;
        _modBusClient.PduFramer = _framerService.PduFramer;
        _modBusClient.PduDeframer = _framerService.PduDeframer;

        try
        {
            await _modBusClient.ConnectAsync(endPoint, cancellationToken);

            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("GivEnergy Client: Connected to end point.");
        }
        catch (ModBusClientException ex)
        {
            _logger.LogError(ex, "GivEnergy Client: Exception while connecting to end point: {ExceptionCode}", ex.ExceptionCode);
            throw;
        }
    }

    public async ValueTask RequestInverterDataAsync(ResponseDataType responseDataType, byte deviceIndex, CancellationToken cancellationToken)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("GivEnergy Client: Requesting read for inverter registers {Type} for device index {Index}.", responseDataType, deviceIndex);

        try
        {
            var deviceAddress = responseDataType.DeviceAddress(deviceIndex);
            var inputRegisters = responseDataType.InputRegisters();
            var startAddress = responseDataType.StartAddress();

            await SendReadRegistersAsync(deviceAddress, inputRegisters, startAddress, cancellationToken);

            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("GivEnergy Client: Requested read for inverter registers {Type} for device index {Index}.", responseDataType, deviceIndex);
        }
        catch (ModBusClientException ex)
        {
            _logger.LogError(ex, "GivEnergy Client: Exception while requesting read of inverter registers: {ExceptionCode}", ex.ExceptionCode);
            throw;
        }
    }

    public async ValueTask SendReadRegistersAsync(byte deviceAddress, bool inputRegisters, ushort startAddress, CancellationToken cancellationToken)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("GivEnergy Client: Requesting read for {Type} registers from {Address} for device address {DeviceAddress}.", inputRegisters ? "Input" : "Holding", startAddress, deviceAddress);

        try
        {
            _modBusClient.TransactionId = TransactionId;
            _modBusClient.UnitIdentifier = deviceAddress;

            if (inputRegisters)
            {
                await _modBusClient.ReadInputRegistersRequestAsync(startAddress, RegisterBlockCount, cancellationToken);
            }
            else
            {
                await _modBusClient.ReadHoldingRegistersRequestAsync(startAddress, RegisterBlockCount, cancellationToken);
            }

            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("GivEnergy Client: Requested read for {Type} registers from {Address} for device address {DeviceAddress}.", inputRegisters ? "Input" : "Holding", startAddress, deviceAddress);
        }
        catch (ModBusClientException ex)
        {
            _logger.LogError(ex, "GivEnergy Client: Exception while requesting read of holding registers: {ExceptionCode}", ex.ExceptionCode);
            throw;
        }
    }

    public async ValueTask<GivEnergyResponse> WaitForResponseAsync(CancellationToken cancellationToken)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("GivEnergy Client: Waiting for register values.");

        try
        {
            var result = await _modBusClient.ReadRegistersResponseDataAsync(cancellationToken);

            var response = _inverterDataConverter.ParseResponse(
                _framerService.SerialNo,
                _framerService.WifiHost,
                _framerService.RegisterAddress,
                result
            );

            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("GivEnergy Client: Read {NumberOfRegisters} type {FunctionNumber} register values starting at address: {StartAddress}.", result.UshortData.Count, result.FunctionNumber, _framerService.RegisterAddress);

            return response;
        }
        catch (ModBusClientException ex)
        {
            _logger.LogError(ex, "GivEnergy Client: Exception while reading register values: {ExceptionCode}", ex.ExceptionCode);
            throw;
        }
    }

    public async ValueTask SendCustomPduAsync(ReadOnlyMemory<byte> pdu, CancellationToken cancellationToken)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("GivEnergy Client: Sending custom PDU.");

        try
        {
            _modBusClient.TransactionId = TransactionId;
            await _modBusClient.SendCustomPduAsync(pdu, cancellationToken);

            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("GivEnergy Client: Sent custom PDU.");
        }
        catch (ModBusClientException ex)
        {
            _logger.LogError(ex, "GivEnergy Client: Exception while sending custom PDU: {ExceptionCode}", ex.ExceptionCode);
            throw;
        }
    }

    public void Close()
    {
        _logger.LogInformation("GivEnergy Client: Closing connection");

        _modBusClient.Close();

        _logger.LogInformation("GivEnergy Client: Closed connection");
    }
}
