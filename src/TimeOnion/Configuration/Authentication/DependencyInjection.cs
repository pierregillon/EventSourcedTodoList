namespace TimeOnion.Configuration.Authentication;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
    {
        services.AddScoped<Authenticator>();
        services.AddScoped<JwtTokenBuilder>();

        services
            .AddOptions<JwtTokenOptions>()
            .BindConfiguration(JwtTokenOptions.SectionName)
            .ValidateDataAnnotations();

        return services;
    }
}