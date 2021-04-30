﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TMF.Corporate.WebApp.AuthorizationPolicies
{
    public class RoleMemberHandler : AuthorizationHandler<RoleMemberRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleMemberRequirement requirement)
        {
            var roleClaims = context
                                .User
                                .Claims
                                .Where(c => c.Type == ClaimTypes.Role);

            if (roleClaims != null)
            {
                foreach (var roleClaim in roleClaims)
                {
                    if (roleClaim != null)
                    {
                        if (roleClaim.Value.Equals(requirement.RoleName,
                                                   StringComparison.InvariantCultureIgnoreCase))
                        {
                            context.Succeed(requirement);
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
