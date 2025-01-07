using CesiZen.Domain.Interface;
using CesiZen.Infrastructure.DatabaseContext;
using CesiZen.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CesiZen.Infrastructure;

public static class InfrastructureRegisterService
{
    public static IServiceCollection AddInfrastructureContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDb")
            ?? throw new InvalidOperationException("Connection string : 'MongoDb' not found.");
        var dbName = configuration.GetValue<string>("ConnectionStrings:MongoDbName");

        services.AddDbContext<MongoDbContext>(options =>
            options.UseMongoDB(connectionString, dbName));


        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddInfrastructureCommandServices();
        services.AddInfrastructureQueryServices();

        return services;
    }

    private static IServiceCollection AddInfrastructureQueryServices(this IServiceCollection services)
    {
        services.AddSingleton<ILoginQuery, LoginQuery>();
        services.AddSingleton<IUserQuery, UserQuery>();

        return services;
    }

    private static IServiceCollection AddInfrastructureCommandServices(this IServiceCollection services)
    {
        services.AddSingleton<ILoginCommand, LoginCommand>();
        services.AddSingleton<IUserCommand, UserCommand>();

        return services;
    }
}
