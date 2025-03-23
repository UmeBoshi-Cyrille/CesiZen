using Newtonsoft.Json;

namespace CesiZen.Domain.Datamodel;

public class Session
{
    public int Id { get; set; }

    public string SessionId { get; set; }

    public string UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }

    public Session(string sessionId, string userId)
    {
        Id = 0;
        SessionId = sessionId;
        UserId = userId;
    }
}