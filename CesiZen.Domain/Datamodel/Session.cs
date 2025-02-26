namespace CesiZen.Domain.Datamodel;

public class Session
{
    public int Id { get; set; }
    public string SessionId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public Session(string sessionId, int userId)
    {
        SessionId = sessionId;
        UserId = userId;
    }
}