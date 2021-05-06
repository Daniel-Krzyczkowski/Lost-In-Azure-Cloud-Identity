using Microsoft.AspNetCore.Authorization;

namespace TMF.Corporate.WebApp.AuthorizationPolicies
{
    public class ClaimRequirement : IAuthorizationRequirement
    {
        public string RoleName { get; set; }

        public ClaimRequirement(string roleName)
        {
            RoleName = roleName;
        }
    }
}
