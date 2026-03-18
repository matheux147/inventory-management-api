using System.Globalization;
using InventoryManagement.Api.Middlewares;
using InventoryManagement.Infrastructure.AuditLog;
using InventoryManagement.Infrastructure.Email;
using InventoryManagement.Infrastructure.Persistence;
using InventoryManagement.Infrastructure.Wms;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(
    builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddPersistence(
    builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddWmsInfrastructure(
    builder.Configuration.GetSection("Integrations:Wms"));

builder.Services.AddAuditLogInfrastructure(
    builder.Configuration.GetSection("Integrations:AuditLog"));

builder.Services.AddEmailInfrastructure(
    builder.Configuration.GetSection("Integrations:Email"));

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

// Add services to the container.
builder.Services
    .AddControllers()
    .AddDataAnnotationsLocalization();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("pt-BR")
};

var app = builder.Build();

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseRequestLocalization(localizationOptions);

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
