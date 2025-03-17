using CesiZen.Application.Services;
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
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAuthenticateService, AuthenticationService>();
        services.AddScoped<IRegisterService, RegisterService>();

        return services;
    }

    private static IServiceCollection AddQueryServices(this IServiceCollection services)
    {
        services.AddScoped<ILoginQueryService, LoginQueryService>();
        services.AddScoped<IUserQueryService, UserQueryService>();

        services.AddScoped<IArticleQueryService, ArticleQueryService>();
        services.AddScoped<ICategoryQueryService, CategoryQueryService>();
        services.AddScoped<IBreathExerciseQueryService, BreathExerciseQueryService>();
        services.AddScoped<IUserQueryService, UserQueryService>();

        return services;
    }

    private static IServiceCollection AddCommandServices(this IServiceCollection services)
    {
        services.AddScoped<ILoginCommandService, LoginCommandService>();
        services.AddScoped<IUserCommandService, UserCommandService>();

        services.AddScoped<ICategoryCommandService, CategoryCommandService>();
        services.AddScoped<IArticleCommandService, ArticleCommandService>();
        services.AddScoped<IBreathExerciseCommandService, BreathExerciseCommandService>();
        services.AddScoped<IUserCommandService, UserCommandService>();

        return services;
    }
}
