using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;

namespace CesiZen.Application.Authorization;

public class RoleManager : AuthorizationHandler<RolesAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        RolesAuthorizationRequirement requirement)
    {
        if (context.User.Identity!.IsAuthenticated)
        {
            var userRoles = context.User.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value);

            if (requirement.AllowedRoles.Any(x => userRoles.Contains(x)))
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}
