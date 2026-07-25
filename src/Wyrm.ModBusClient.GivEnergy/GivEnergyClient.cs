using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using Wyrm.ModBusClient.GivEnergy.Extensions;
using Wyrm.ModBusClient.GivEnergy.Responses.Constants;
using Wyrm.ModBusClient.GivEnergy.Services;

namespace Wyrm.ModBusClient.GivEnergy;

internal sealed class GivEnergyClient(
    IModBusRegisterClient _modBusClient,
    IInverterDataConverter _inverterDataConverter,
    ILogger<GivEnergyClient> _logger) : IGivEnergyClient
{
    private const ushort ProtocolIdentifier = 0x0001;
    private const ushort TransactionId = 0x5959;
    private const byte GivUnitId = 0x01;
    private const byte GivFuncNo = 0x02;
    private const ushort RegisterBlockCount = 60;
    private static readonly byte[] CommandPadding = new byte[16];

    private string _wifiHost = string.Empty;
    private string _serialNo = string.Empty;
    private int _registerAddress;

    public async ValueTask ConnectAsync(EndPoint endPoint, CancellationToken cancellationToken = default)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("GivEnergy Client: Connecting to end point: {Address}", endPoint.Serialize().ToString());

        _modBusClient.ProtocolIdentifier = ProtocolIdentifier;
        _modBusClient.PduFramer = PduFramer;
        _modBusClient.PduDeframer = PduDeframer;

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
                await _modBusClient.ReadHoldingRegistersRequestAsync(startAddress, RegisterBlockCount, cancellationToken);
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
                _serialNo,
                _wifiHost,
                _registerAddress,
                result
            );

            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("GivEnergy Client: Read {NumberOfRegisters} type {FunctionNumber} register values starting at address: {StartAddress}.", result.UshortData.Count, result.FunctionNumber, _registerAddress);

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

    private IList<byte> PduFramer(IList<byte> command)
    {
        var count = command.Count + 2;
        var givCommand = new List<byte>([GivUnitId, GivFuncNo, ..CommandPadding, (byte)(count >> 8), (byte)(count & 0xff)]);
        givCommand.AddRange(command);
        givCommand.AddRange(CheckSum(command));
        return givCommand;
    }

    private ReadOnlyMemory<byte> PduDeframer(ReadOnlyMemory<byte> givResponse)
    {
        const int wifiHostStartPosition = 2;
        const int unitIdentifierPosition = 20;
        const int serialNoStartPosition = 22;
        const int stringLength = 10;
        const int regAddressHighPosition = 32;
        const int regAddressLowPosition = 33;
        const int regCountHighPosition = 34;
        const int regCountLowPosition = 35;
        const int registerValuesStartPosition = 36;

        var responseSpan = givResponse.Span;
        if (responseSpan.Length == 13 && responseSpan[0] == 1 && responseSpan[1] == 1 && responseSpan[^1] == 1)
            throw new GivEnergyClientException("Heartbeat received.", givResponse);

        // TODO: Check for other responses other than registers

        var numRegisters = (responseSpan[regCountHighPosition] << 8) + responseSpan[regCountLowPosition];
        // TODO: Check number of registers

        var checkSum = CheckSum(responseSpan[unitIdentifierPosition..^2].ToArray());
        _logger.LogInformation($"GivEnergy Client: Checksum: {responseSpan[responseSpan.Length - 2]} {responseSpan[responseSpan.Length - 1]} = {checkSum[0]} {checkSum[1]}");
        // TODO: Check CheckSum: everything after length but not checksum itself of course

        try
        {
            _wifiHost = Encoding.ASCII.GetString([.. givResponse.Slice(wifiHostStartPosition, stringLength).TrimEnd((byte)0).ToArray()]);
            _serialNo = Encoding.ASCII.GetString([.. givResponse.Slice(serialNoStartPosition, stringLength).TrimEnd((byte)0).ToArray()]);
            var response = new List<byte>();
            response.AddRange(givResponse.Slice(unitIdentifierPosition, 2).Span);
            _registerAddress = (responseSpan[regAddressHighPosition] << 8) + responseSpan[regAddressLowPosition];
            var bytes = numRegisters * 2;
            response.Add((byte)bytes);
            response.AddRange(givResponse.Slice(registerValuesStartPosition, bytes).Span);
            return new ReadOnlyMemory<byte>([.. response]);
        }
        catch (Exception ex)
        {
            throw new GivEnergyClientException($"Error decoding data frame: {string.Join(' ', givResponse.ToArray().Select(b => $"{b:X2}"))}", ex);
        }
    }

    private static byte[] CheckSum(ICollection<byte> data)
    {
        ushort crc = 0xFFFF;
        foreach (byte b in data)
        {
            crc ^= b;
            for (int i = 0; i < 8; i++)
            {
                bool lsbSet = (crc & 0x0001) != 0;
                crc >>= 1;
                if (lsbSet)
                    crc ^= 0xA001;
            }
        }

        return [(byte)(crc & 0xff), (byte)(crc >> 8)];
    }
}
