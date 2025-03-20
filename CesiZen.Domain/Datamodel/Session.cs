using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CesiZen.Domain.Datamodel;

public class Session
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("sessionId")]
    public string SessionId { get; set; } = string.Empty;

    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;

    [JsonIgnore]
    public User? User { get; set; }

    public Session(string sessionId, string userId)
    {
        Id = string.Empty;
        SessionId = sessionId;
        UserId = userId;
    }
}