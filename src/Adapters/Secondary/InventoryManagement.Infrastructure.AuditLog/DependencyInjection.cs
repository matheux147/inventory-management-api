using InventoryManagement.Domain.Ports.Gateways.Audit;
using InventoryManagement.Infrastructure.AuditLog.Options;
using InventoryManagement.Infrastructure.AuditLog.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace InventoryManagement.Infrastructure.AuditLog;

public static class DependencyInjection
{
    public static IServiceCollection AddAuditLogInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<AuditLogOptions>()
            .Bind(configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHttpClient<IAuditLogGateway, AuditLogGateway>((serviceProvider, httpClient) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<AuditLogOptions>>().Value;

            httpClient.BaseAddress = new Uri(options.BaseUrl);
            httpClient.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        });

        return services;
    }
}