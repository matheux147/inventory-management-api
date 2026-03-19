using System.Globalization;
using InventoryManagement.Api.Middlewares;
using Microsoft.AspNetCore.Localization;

namespace InventoryManagement.Api.Dependencies;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseProjectDependencies(this WebApplication app)
    {
        var supportedCultures = new[]
        {
            new CultureInfo("en-US"),
            new CultureInfo("pt-BR")
        };

        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en-US"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        };

        app.UseRequestLocalization(localizationOptions);

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory Management API v1");
                options.RoutePrefix = "swagger";
                options.DocumentTitle = "Inventory Management API";
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}