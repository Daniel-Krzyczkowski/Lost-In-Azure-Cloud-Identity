using Microsoft.AspNetCore.Authorization;

namespace TMF.Corporate.WebApp.AuthorizationPolicies
{
    public class ClaimRequirement : IAuthorizationRequirement
    {
        public string ClaimName { get; set; }

        public ClaimRequirement(string claimName)
        {
            ClaimName = claimName;
        }
    }
}
