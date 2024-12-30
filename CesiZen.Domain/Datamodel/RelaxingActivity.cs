namespace CesiZen.Domain.Datamodel;

public class RelaxingActivity : ADate
{
    public string Content { get; set; } = string.Empty;

    public Image? Presentation { get; set; } = new();

    public Category? Category { get; set; }

    public bool Favourite { get; set; }

    public ICollection<Video>? Videos { get; set; }

    public ICollection<Image>? Images { get; set; }
}
