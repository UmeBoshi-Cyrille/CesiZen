namespace CesiZen.Domain.DataTransfertObject;

public record UserDto : UserMinimumDto
{
    public LoginMinimumDto? Login { get; set; }
}
