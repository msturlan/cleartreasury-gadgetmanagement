using Microsoft.Extensions.Options;

namespace ClearTreasury.GadgetManagement.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static OptionsBuilder<TOptions> AddBoundValidatedOptions<TOptions>(
        this IServiceCollection services, string configSectionPath)
        where TOptions : class
    {
        return services.AddOptions<TOptions>()
            .BindConfiguration(configSectionPath)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    public static OptionsBuilder<TOptions> AddConfiguredValidatedOptions<TOptions>(
            this IServiceCollection services, Action<TOptions> configureOptions)
            where TOptions : class
    {
        return services.AddOptions<TOptions>()
            .Configure(configureOptions)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
