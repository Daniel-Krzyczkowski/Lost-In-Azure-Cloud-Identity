using Microsoft.Extensions.Options;
using System;
using TMF.Identity.Infrastructure.Configuration.Interfaces;

namespace TMF.Identity.Infrastructure.Configuration
{
    public class MsGraphServiceConfiguration : IMsGraphServiceConfiguration
    {
        public string TenantId { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string TenantName { get; set; }
    }

    public class MsGraphServiceConfigurationValidation : IValidateOptions<MsGraphServiceConfiguration>
    {
        public ValidateOptionsResult Validate(string name, MsGraphServiceConfiguration options)
        {
            if (string.IsNullOrEmpty(options.TenantId))
            {
                throw new ArgumentNullException($"{nameof(options.TenantId)} configuration parameter for the Microsoft Graph Service is required");
            }

            if (string.IsNullOrEmpty(options.AppId))
            {
                throw new ArgumentNullException($"{nameof(options.AppId)} configuration parameter for the Microsoft Graph Service is required");
            }

            if (string.IsNullOrEmpty(options.AppSecret))
            {
                throw new ArgumentNullException($"{nameof(options.AppSecret)} configuration parameter for the Microsoft Graph Service is required");
            }

            if (string.IsNullOrEmpty(options.TenantName))
            {
                throw new ArgumentNullException($"{nameof(options.TenantName)} configuration parameter for the Microsoft Graph Service is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
