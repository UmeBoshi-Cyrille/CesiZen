using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.Datamodel;

public class Image
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;

    public string ArticleId { get; set; } = string.Empty;
    public Article? Article { get; set; }
}
