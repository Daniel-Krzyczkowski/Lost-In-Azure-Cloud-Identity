using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TMF.Identity.Infrastructure.Configuration;
using TMF.Identity.Infrastructure.Configuration.Interfaces;

namespace TMF.Identity.API.Core.DependencyInjection
{
    static class ConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection AddAppConfiguration(this IServiceCollection services,
                                                                                     IConfiguration configuration)
        {
            services.Configure<ApplicationInsightsServiceConfiguration>(configuration.GetSection("ApplicationInsights"));
            services.AddSingleton<IValidateOptions<ApplicationInsightsServiceConfiguration>, ApplicationInsightsServiceConfigurationValidation>();
            var applicationInsightsServiceConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<ApplicationInsightsServiceConfiguration>>().Value;
            services.AddSingleton<IApplicationInsightsServiceConfiguration>(applicationInsightsServiceConfiguration);

            services.Configure<MsGraphServiceConfiguration>(configuration.GetSection("MicrosoftGraph"));
            services.AddSingleton<IValidateOptions<MsGraphServiceConfiguration>, MsGraphServiceConfigurationValidation>();
            var msGraphServiceConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<MsGraphServiceConfiguration>>().Value;
            services.AddSingleton<IMsGraphServiceConfiguration>(msGraphServiceConfiguration);

            return services;
        }
    }
}
