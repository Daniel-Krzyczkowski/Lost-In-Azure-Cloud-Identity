using Microsoft.AspNetCore.Authorization;

namespace TMF.Corporate.WebApp.AuthorizationPolicies
{
    public class RoleMemberRequirement : IAuthorizationRequirement
    {
        public string RoleName { get; set; }

        public RoleMemberRequirement(string roleName)
        {
            RoleName = roleName;
        }
    }
}
