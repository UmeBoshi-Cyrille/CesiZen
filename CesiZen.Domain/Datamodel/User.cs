using CesiZen.Domain.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.Datamodel;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [MaxLength(50)]
    [MinLength(2)]
    [RegularExpression(@"^[a-zA-Z]+$")]
    [BsonElement("firstName")]
    public string Firstname { get; set; } = string.Empty;

    [MaxLength(50)]
    [MinLength(2)]
    [RegularExpression(@"^[a-zA-Z]+$")]
    [BsonElement("lastName")]
    public string Lastname { get; set; } = string.Empty;

    [MaxLength(50)]
    [MinLength(2)]
    [RegularExpression(@"^[a-zA-Z]+$")]
    [BsonElement("username")]
    public string? Username { get; set; } = string.Empty;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("lastLoginAt")]
    public DateTime? LastLoginAt { get; set; }

    [BsonElement("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [BsonElement("isActive")]
    public bool IsActive { get; set; }

    [BsonElement("role")]
    public RoleType Role { get; set; }

    [BsonElement("refreshTokenExpiryTime")]
    public DateTime? RefreshTokenExpiryTime { get; set; }

    [BsonElement("login")]
    public Login? Login { get; set; }

    [BsonElement("refreshToken")]
    public RefreshToken? RefreshToken { get; set; }

    [BsonElement("session")]
    public Session? Session { get; set; }
}
