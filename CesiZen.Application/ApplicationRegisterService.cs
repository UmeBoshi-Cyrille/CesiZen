using CesiZen.Application.Services;
using CesiZen.Domain.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace CesiZen.Application;

public static class ApplicationRegisterService
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddApplicationCommandServices();
        services.AddApplicationQueryServices();

        return services;
    }

    private static IServiceCollection AddApplicationQueryServices(this IServiceCollection services)
    {
        services.AddSingleton<IUserQueryService, UserQueryService>();
        services.AddSingleton<ILoginQueryService, LoginQueryService>();

        return services;
    }

    private static IServiceCollection AddApplicationCommandServices(this IServiceCollection services)
    {
        services.AddSingleton<IUserCommandService, UserCommandService>();
        services.AddSingleton<ILoginCommandService, LoginCommandService>();

        return services;
    }
}
