using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using CesiZen.Infrastructure.Notifiers;
using CesiZen.Infrastructure.Providers;
using CesiZen.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CesiZen.Infrastructure;

public static class ServiceRegister
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddInfrastructureCommandServices();
        services.AddInfrastructureQueryServices();

        return services;
    }

    public static IServiceCollection AddInfrastructureContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDb")
            ?? throw new InvalidOperationException("Connection string : 'MongoDb' not found.");
        var dbName = configuration.GetValue<string>("ConnectionStrings:MongoDbName");

        services.AddDbContext<MongoDbContext>(options =>
            options.UseMongoDB(connectionString, dbName!));


        return services;
    }

    private static IServiceCollection AddInfrastructureQueryServices(this IServiceCollection services)
    {
        services.AddScoped<ISessionQuery, SessionQuery>();
        services.AddScoped<IRefreshTokenQuery, RefreshTokenQuery>();
        services.AddScoped<IUserQuery, UserQuery>();
        services.AddScoped<ILoginQuery, LoginQuery>();

        services.AddScoped<IArticleQuery, ArticleQuery>();
        services.AddScoped<ICategoryQuery, CategoryQuery>();
        services.AddScoped<IBreathExerciseQuery, BreathExerciseQuery>();
        services.AddScoped<IUserQuery, UserQuery>();

        return services;
    }

    private static IServiceCollection AddInfrastructureCommandServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddSingleton<INotifier, MessageNotifier>();
        services.AddTransient<IObserver, EmailService>();

        services.AddScoped<ISessionCommand, SessionCommand>();
        services.AddScoped<IRefreshTokenCommand, TokenCommand>();
        services.AddScoped<IUserCommand, UserCommand>();
        services.AddScoped<ILoginCommand, LoginCommand>();

        services.AddScoped<ICategoryCommand, CategoryCommand>();
        services.AddScoped<IArticleCommand, ArticleCommand>();
        services.AddScoped<IBreathExerciseCommand, BreathExerciseCommand>();
        services.AddScoped<IUserCommand, UserCommand>();

        return services;
    }
}
