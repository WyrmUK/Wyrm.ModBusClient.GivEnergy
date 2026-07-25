using Microsoft.Extensions.DependencyInjection;
using Wyrm.ModBusClient.DependencyInjection;
using Wyrm.ModBusClient.GivEnergy.Services;

namespace Wyrm.ModBusClient.GivEnergy.DependencyInjection;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the required services for the GivEnergy client.
    /// </summary>
    /// <param name="services">An <see cref="IServiceCollection"/> to add to.</param>
    /// <param name="serviceLifetime">The <see cref="ServiceLifetime"/> for the client (defaults to Singleton).</param>
    /// <returns>The <see cref="IServiceCollection"/> with the added GivEnergy client services.</returns>
    public static IServiceCollection AddGivEnergyClient(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
    {
        services.AddModBusClient(serviceLifetime);
        services.AddSingleton<IInverterDataConverter, InverterDataConverter>();
        services.Add(new ServiceDescriptor(typeof(IGivEnergyClient), typeof(GivEnergyClient), serviceLifetime));
        return services;
    }
}
