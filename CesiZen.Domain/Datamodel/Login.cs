using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.Datamodel;

public class Login
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Required(ErrorMessage = "Email is Required")]
    [EmailAddress(ErrorMessage = "Invalid Email format")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,3}$",
        ErrorMessage = "Invalid Email format")]
    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;

    [MaxLength(150)]
    [MinLength(12)]
    [Required(ErrorMessage = "Invalid Password")]
    [RegularExpression(@"^(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?])(?=.*[a-zA-Z])(?=.*\d)(?=.{12,})[^\s]+$",
        ErrorMessage = "Invalid password format")]
    [BsonElement("password")]
    public string Password { get; set; } = string.Empty;

    [BsonElement("emailVerified")]
    public bool EmailVerified { get; set; } = false;

    [BsonElement("emailVerificationToken")]
    public string EmailVerificationToken { get; set; }

    [BsonElement("salt")]
    public string Salt { get; set; } = string.Empty;

    [BsonElement("accessFailedCount")]
    public int AccessFailedCount { get; set; }

    [BsonElement("locked")]
    public bool IsLocked { get; set; }

    [BsonElement("lockoutTime")]
    [DataType(DataType.DateTime)]
    public DateTime? LockoutEndTime { get; set; }

    [JsonIgnore]
    public string UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; }
}
