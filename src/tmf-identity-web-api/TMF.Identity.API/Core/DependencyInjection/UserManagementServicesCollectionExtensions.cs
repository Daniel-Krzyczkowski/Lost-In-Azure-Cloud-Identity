using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using TMF.Identity.Core.Interfaces;
using TMF.Identity.Infrastructure.Configuration.Interfaces;
using TMF.Identity.Infrastructure.Services;

namespace TMF.Identity.API.Core.DependencyInjection
{
    static class UserManagementServicesCollectionExtensions
    {
        public static IServiceCollection AddUserManagementServices(this IServiceCollection services)
        {
            services.AddSingleton<IGraphServiceClient>(implementationFactory =>
            {
                var msGraphServiceConfiguration = implementationFactory.GetRequiredService<IMsGraphServiceConfiguration>();
                IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(msGraphServiceConfiguration.AppId)
                    .WithTenantId(msGraphServiceConfiguration.TenantId)
                    .WithClientSecret(msGraphServiceConfiguration.AppSecret)
                    .Build();

                ClientCredentialProvider authProvider = new ClientCredentialProvider(confidentialClientApplication);
                return new GraphServiceClient(authProvider);
            });

            services.AddSingleton<IUserManagementService, MsGraphUserManagementService>();

            return services;
        }
    }
}
