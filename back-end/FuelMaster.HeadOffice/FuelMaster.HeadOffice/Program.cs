using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Extensions.Dependencies;
using FuelMaster.HeadOffice.Extensions.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterDependencies(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.ApplyMigrationForAllTenants();

app.UseMiddlewares();
await app.UseSeedersAsync();


app.Run();

