using CesiZen.Domain.Enum;

namespace CesiZen.Domain.Datamodel;

public class User : AUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public Login Login { get; set; } = new();

    public RoleType Role { get; set; }
}
