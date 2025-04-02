using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CesiZen.Domain.Datamodel;

public class Image
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    public string Alternative { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public int ArticleId { get; set; }

    [JsonIgnore]
    public Article? Article { get; set; }
}
