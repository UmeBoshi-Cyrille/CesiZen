using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.Datamodel;

public class Article
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("createdDate")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updatedDate")]
    public DateTime UpdatedAt { get; set; }

    [MaxLength(255)]
    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(3000)]
    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [MaxLength(100)]
    [BsonElement("author")]
    public string Author { get; set; } = string.Empty;

    [BsonElement("content")]
    public string Content { get; set; } = string.Empty;

    public Image? Image { get; set; }

    public ICollection<Image>? Images { get; set; }

    public Category? Category { get; set; }
}
