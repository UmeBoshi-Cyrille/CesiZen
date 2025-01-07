using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CesiZen.Infrastructure;

public static class InfrastructureRegisterService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDb")
            ?? throw new InvalidOperationException("Connection string : 'MongoDb' not found.");
        var dbName = configuration.GetValue<string>("ConnectionStrings:MongoDbName");

        services.AddDbContext<MongoDbContext>(options =>
            options.UseMongoDB(connectionString, dbName));


        return services;
    }
}
