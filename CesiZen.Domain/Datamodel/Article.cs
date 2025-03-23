using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.Datamodel;

public class Article
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public Image? Presentation { get; set; }

    public ICollection<Image>? Images { get; set; }

    public ICollection<Category>? Category { get; set; }
}
