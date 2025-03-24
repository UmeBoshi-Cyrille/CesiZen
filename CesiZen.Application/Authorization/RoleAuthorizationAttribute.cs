using Microsoft.AspNetCore.Authorization;

namespace CesiZen.Application.Authorization;

public class RoleAuthorizationAttribute : AuthorizeAttribute
{
    public RoleAuthorizationAttribute(params string[] roles) : base()
    {
        Roles = string.Join(",", roles);
    }
}
