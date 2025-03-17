using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.DataTransfertObject;

public record ArticleDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }
    public Image Image { get; set; }
    public ICollection<Image> Images { get; set; }
}
