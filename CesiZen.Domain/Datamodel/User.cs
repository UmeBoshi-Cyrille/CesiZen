using CesiZen.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.Datamodel;

public class User : IdentityUser
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("firstName")]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [BsonElement("lastName")]
    [MaxLength(50)]
    public string LastName { get; set; }

    [BsonElement("username")]
    [MaxLength(50)]
    public string? Username { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("lastLoginAt")]
    public DateTime? LastLoginAt { get; set; }

    [BsonElement("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [BsonElement("isActive")]
    public bool IsActive { get; set; }

    [BsonElement("role")]
    public RoleType Role { get; set; } = RoleType.NONE;

    [BsonElement("refreshTokenExpiryTime")]
    public DateTime? RefreshTokenExpiryTime { get; set; }

    [BsonElement("login")]
    public Login Login { get; set; }

    [BsonElement("refreshToken")]
    public RefreshToken? RefreshToken { get; set; }

    [BsonElement("session")]
    public Session? Session { get; set; }
}
