using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.DataTransfertObject;

public class NewArticleDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string PresentationImagePath { get; set; } = string.Empty;
    public ICollection<Image>? Images { get; set; }
}
