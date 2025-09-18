using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Extensions.Dependencies;
using FuelMaster.HeadOffice.Extensions.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterDependencies(builder.Configuration);

// Add Serilog
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.ApplyMigrationForAllTenants();

app.UseMiddlewares();
await app.UseSeedersAsync();


app.Run();

// Ensure to flush and close the log
Log.CloseAndFlush();

