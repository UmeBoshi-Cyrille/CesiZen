using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CesiZen.Domain.Datamodel;

public class Session
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }

    [BsonElement("sessionId")]
    public string SessionId { get; set; }

    [JsonIgnore]
    public int UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; }

    public Session(string sessionId, int userId)
    {
        SessionId = sessionId;
        UserId = userId;
    }
}