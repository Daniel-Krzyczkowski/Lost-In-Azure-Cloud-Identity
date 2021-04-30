﻿using Microsoft.Extensions.Options;
using TMF.Identity.Infrastructure.Configuration.Interfaces;

namespace TMF.Identity.Infrastructure.Configuration
{
    public class ApplicationInsightsServiceConfiguration : IApplicationInsightsServiceConfiguration
    {
        public string InstrumentationKey { get; set; }
    }

    public class ApplicationInsightsServiceConfigurationValidation : IValidateOptions<ApplicationInsightsServiceConfiguration>
    {
        public ValidateOptionsResult Validate(string name, ApplicationInsightsServiceConfiguration options)
        {
            if (string.IsNullOrEmpty(options.InstrumentationKey))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.InstrumentationKey)} configuration parameter for the Azure Application Insights is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
