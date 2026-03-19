using InventoryManagement.Api.Dependencies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProjectDependencies(builder.Configuration);

var app = builder.Build();

app.UseProjectDependencies();

app.Run();