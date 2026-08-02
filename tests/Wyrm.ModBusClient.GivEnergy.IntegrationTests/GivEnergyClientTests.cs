using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Sockets;
using Wyrm.ModBusClient.GivEnergy.DependencyInjection;

namespace Wyrm.ModBusClient.GivEnergy.IntegrationTests;

public class GivEnergyClientTests
{
    #region Setup

    private readonly IGivEnergyClient _givEnergyClient;
    private readonly System.Net.Sockets.Socket _inverter = new(TestEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

    private static readonly EndPoint TestEndPoint = new IPEndPoint(new IPAddress([127, 0, 0, 1]), 8000);

    public GivEnergyClientTests()
    {
        var services = new ServiceCollection().AddGivEnergyClient();
        var provider = services.BuildServiceProvider();
        _givEnergyClient = provider.GetRequiredService<IGivEnergyClient>();
        _inverter.Bind(TestEndPoint);
        _inverter.Listen();
    }

    #endregion

    [Fact]
    public async Task ConnectAsync_Should_Connect()
    {
        var accept = Task.Run(() => _inverter.AcceptAsync(TestContext.Current.CancellationToken));
        await _givEnergyClient.ConnectAsync(TestEndPoint, TestContext.Current.CancellationToken);
        await accept;
        _givEnergyClient.Close();
    }
}
