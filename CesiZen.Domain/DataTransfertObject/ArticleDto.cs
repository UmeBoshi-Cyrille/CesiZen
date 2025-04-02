using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.DataTransfertObject;

public record ArticleDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public ICollection<Image>? Images { get; set; }
    public ICollection<Category>? Categories { get; set; }
}
