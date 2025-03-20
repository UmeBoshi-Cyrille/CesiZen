using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.DataTransfertObject;

public record ArticleDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Image? Image { get; set; }
    public ICollection<Image>? Images { get; set; }
}
