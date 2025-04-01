using CesiZen.Api;
using CesiZen.Application;
using CesiZen.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

string environmentName = builder.Environment.EnvironmentName;
Console.WriteLine($"Current environment: {environmentName}");

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .Build();

// Add services to the container.
builder.Services.AddConfigurationServices(builder.Configuration);
builder.Services.AddInfrastructureContext(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseCors("AllowAngularClient");
app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Application Started");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The applciation failed to start correctly");
}
finally
{
    Log.CloseAndFlush();
}