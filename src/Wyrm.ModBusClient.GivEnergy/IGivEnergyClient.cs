using System.Net;
using Wyrm.ModBusClient.GivEnergy.Responses.Constants;

namespace Wyrm.ModBusClient.GivEnergy;

/// <summary>
/// Interface for a TCP ModBus GivEnergy client.
/// </summary>
public interface IGivEnergyClient
{
    /// <summary>
    /// Connects the client to a GivEnergy server.
    /// </summary>
    /// <param name="endPoint">The <see cref="IPEndPoint"/> to connect to.</param>
    /// <param name="cancellationToken">Token to cancel the connect.</param>
    /// <exception cref="ModBusClientException">Thrown if there is an error.</exception>
    ValueTask ConnectAsync(EndPoint endPoint, CancellationToken cancellationToken = default);
    /// <summary>
    /// Requests inverter data for a specific response type and device index.
    /// </summary>
    /// <param name="responseDataType">The <see cref="ResponseDataType"/> to fetch (RegisterData is not supported).</param>
    /// <param name="deviceIndex">The index of the device to request for (0-based).</param>
    /// <param name="cancellationToken">Token to cancel the connect.</param>
    /// <exception cref="ModBusClientException">Thrown if there's an issue when sending.</exception>
    /// <exception cref="GivEnergyClientException">Thrown if the request is invalid.</exception>
    ValueTask RequestInverterDataAsync(ResponseDataType responseDataType = ResponseDataType.InverterProperties1, byte deviceIndex = 0, CancellationToken cancellationToken = default);
    /// <summary>
    /// Sends a command to the GivEnergy server to read 60 registers.
    /// </summary>
    /// <param name="deviceAddress">The inverter sub-module address (defaults to 17).</param>
    /// <param name="inputRegisters">True to request the input registers (data), false for the holding registers (properties).</param>
    /// <param name="startAddress">The register to start reading the block of 60 from (defaults to 0).</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <exception cref="ModBusClientException">Thrown if there's an issue when sending.</exception>
    ValueTask SendReadRegistersAsync(byte deviceAddress = 0x11, bool inputRegisters = false, ushort startAddress = 0, CancellationToken cancellationToken = default);
    /// <summary>
    /// Waits for a response and returns a partially populated <see cref="GivEnergyResponse"/>
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the wait.</param>
    /// <returns>An <see cref="GivEnergyResponse"/> with specific data populated.</returns>
    /// <exception cref="ModBusClientException">Thrown if there's an issue when reading.</exception>
    /// <exception cref="GivEnergyClientException">Thrown if the data can't be deserialised - might be a heartbeat response.</exception>
    ValueTask<GivEnergyResponse> WaitForResponseAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Sends a custom PDU to the server.
    /// The TransactionId, Protocol Identifier, and data length are prepended.
    /// </summary>
    /// <param name="pdu">The custom PDU to send.</param>
    /// <param name="cancellationToken">Token to cancel the send.</param>
    /// <exception cref="ModBusClientException">Thrown if there's an issue when sending.</exception>
    ValueTask SendCustomPduAsync(ReadOnlyMemory<byte> pdu, CancellationToken cancellationToken = default);
    /// <summary>
    /// Closes the client connection to a GivEnergy server.
    /// </summary>
    void Close();
}
