using CesiZen.Domain.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.Datamodel;

public class BreathExercise
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [MaxLength(100)]
    [BsonElement("title")]
    public string Title { get; set; }

    [Range(1, 600)]
    [BsonElement("timer")]
    public int? Time { get; set; }

    [BsonElement("exerciseType")]
    public ExerciceType ExerciseType { get; set; }

    [BsonElement("userId")]
    public string UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; }
}
