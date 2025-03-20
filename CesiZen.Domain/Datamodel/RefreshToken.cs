using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CesiZen.Domain.Datamodel;

public class RefreshToken
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("refreshToken")]
    public string Token { get; set; } = string.Empty;

    [BsonElement("refreshTokenExpiryTime")]
    public DateTime ExpirationTime { get; set; }

    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;

    [JsonIgnore]
    public User? User { get; set; }
}