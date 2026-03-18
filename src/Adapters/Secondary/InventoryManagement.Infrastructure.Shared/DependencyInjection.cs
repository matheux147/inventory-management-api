using InventoryManagement.Domain.Ports.Context;
using InventoryManagement.Infrastructure.Shared.Context;
using InventoryManagement.Infrastructure.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.Infrastructure.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserContext, CurrentUserContext>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
