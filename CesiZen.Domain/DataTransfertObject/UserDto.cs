namespace CesiZen.Domain.DataTransfertObject;

public record UserDto : UserMinimumDto
{
    public LoginDto? Login { get; set; }
}
