using InventoryManagement.Application;
using InventoryManagement.Infrastructure.AuditLog;
using InventoryManagement.Infrastructure.Email;
using InventoryManagement.Infrastructure.Persistence;
using InventoryManagement.Infrastructure.Shared;
using InventoryManagement.Infrastructure.Wms;
using Microsoft.OpenApi;

namespace InventoryManagement.Api.Dependencies;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProjectDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplication();

        services.AddPersistence(
            configuration.GetConnectionString("DefaultConnection")!);

        services.AddWmsInfrastructure(
            configuration.GetSection("Integrations:Wms"));

        services.AddAuditLogInfrastructure(
            configuration.GetSection("Integrations:AuditLog"));

        services.AddEmailInfrastructure(
            configuration.GetSection("Integrations:Email"));

        services.AddSharedInfrastructure(configuration);

        services.AddLocalization(options =>
        {
            options.ResourcesPath = "Resources";
        });

        services
            .AddControllers()
            .AddDataAnnotationsLocalization();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Inventory Management API",
                Version = "v1",
                Description = "API for categories, suppliers and products management."
            });

            var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }
}