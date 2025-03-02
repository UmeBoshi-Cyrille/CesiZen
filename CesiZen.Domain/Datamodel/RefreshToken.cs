using CesiZen.Domain.Datamodel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

public class RefreshToken
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("refreshToken")]
    public string Token { get; set; }

    [BsonElement("refreshTokenExpiryTime")]
    public DateTime ExpirationTime { get; set; }

    [JsonIgnore]
    public string UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
}