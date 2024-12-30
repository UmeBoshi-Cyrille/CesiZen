namespace CesiZen.Domain.Datamodel;

public class Information : ADate
{
    public string Content { get; set; } = string.Empty;

    public Image? Presentation { get; set; }

    public ICollection<Image>? Images { get; set; }

    public Category? Category { get; set; }
}
