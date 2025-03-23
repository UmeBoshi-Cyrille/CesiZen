using System.ComponentModel;

namespace CesiZen.Domain.DataTransfertObject;

public class AuthenticateRequestDto
{
    [DefaultValue("user@example.com")]
    public required int Identifier { get; set; }

    [DefaultValue("password")]
    public required string Password { get; set; }
}
