using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.Datamodel;

public class Image
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [MaxLength(100)]
    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("path")]
    public string Path { get; set; }

    public string ArticleId { get; set; }
    public Article Article { get; set; }
}
