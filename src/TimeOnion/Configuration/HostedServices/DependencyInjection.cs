namespace TimeOnion.Configuration.HostedServices;

public static class DependencyInjection
{
    public static IServiceCollection AddCommitterHostedService(this IServiceCollection services) => services
        .AddHostedService<CommitterHostedService>()
        .AddOptions<HostedServicesConfiguration>()
        .BindConfiguration(HostedServicesConfiguration.SectionName)
        .ValidateDataAnnotations().Services;
}