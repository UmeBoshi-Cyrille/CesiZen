namespace CesiZen.Domain.Datamodel;

public class Tracker : AId
{
    public DateTime Date { get; set; }

    public Emotion Emotion { get; set; } = new();
}
