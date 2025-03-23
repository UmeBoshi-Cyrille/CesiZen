using Newtonsoft.Json;

namespace CesiZen.Domain.Datamodel;

public class RefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime ExpirationTime { get; set; }

    public string UserId { get; set; } = string.Empty;

    [JsonIgnore]
    public User? User { get; set; }
}