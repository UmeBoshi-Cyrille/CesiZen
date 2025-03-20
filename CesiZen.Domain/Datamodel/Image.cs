using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.Datamodel;

public class Image
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [MaxLength(100)]
    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("path")]
    public string Path { get; set; } = string.Empty;

    public string ArticleId { get; set; } = string.Empty;
    public Article? Article { get; set; }
}
