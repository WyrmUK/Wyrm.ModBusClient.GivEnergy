using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Wyrm.ModBusClient.GivEnergy.DependencyInjection;
using Wyrm.ModBusClient.GivEnergy.Services;

namespace Wyrm.ModBusClient.GivEnergy.UnitTests.DependencyInjection;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddGivEnergyClient_Should_Add_GivEnergyClient()
    {
        var givEnergyServices = new ServiceCollection();
        var services = givEnergyServices.AddGivEnergyClient(ServiceLifetime.Scoped);

        services.ShouldContain(s => s.Lifetime == ServiceLifetime.Singleton && s.ServiceType == typeof(IInverterDataConverter) && s.ImplementationType == typeof(InverterDataConverter));
        services.ShouldContain(s => s.Lifetime == ServiceLifetime.Scoped && s.ServiceType == typeof(IGivEnergyClient) && s.ImplementationType == typeof(GivEnergyClient));
    }
}
