using CesiZen.Application.Services;
using CesiZen.Domain.Interface;
using CesiZen.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CesiZen.Application;

public static class ServiceRegister
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddQueryServices();
        services.AddCommandServices();
        services.AddServices();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordService, PasswordService>();
        services.AddScoped<IAuthenticateService, AuthenticationService>();
        services.AddScoped<IRegisterService, RegisterService>();

        return services;
    }

    private static IServiceCollection AddQueryServices(this IServiceCollection services)
    {
        services.AddScoped<ILoginQueryService, LoginQueryService>();
        services.AddScoped<IUserQueryService, UserQueryService>();

        return services;
    }

    private static IServiceCollection AddCommandServices(this IServiceCollection services)
    {
        services.AddScoped<ILoginCommandService, LoginCommandService>();
        services.AddScoped<IUserCommandService, UserCommandService>();

        return services;
    }
}
