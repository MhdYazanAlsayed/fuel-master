using FuelMaster.HeadOffice.Extensions.Dependencies;
using FuelMaster.HeadOffice.Extensions.Middlewares;
using FuelMaster.HeadOffice.Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterDependencies(builder.Configuration);

// Add Serilog
builder.Host.UseSerilog();

var app = builder.Build();

app.UseMiddlewares();

// Get tenants from FuelMasterAPI and store them into ITenants
await app.GetTenantsFromFuelMasterAPIAsync();

// Configure the HTTP request pipeline.
app.ApplyMigrationForAllTenants();

await app.UseSeedersAsync();

app.Run();

// Ensure to flush and close the log
Log.CloseAndFlush();

