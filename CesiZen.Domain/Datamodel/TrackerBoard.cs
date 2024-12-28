namespace CesiZen.Domain.Datamodel;

public class TrackerBoard : AId
{
    public int UserId { get; set; }

    public ICollection<Tracker>? Trackers { get; set; }

    public ICollection<Period>? Periods { get; set; }
}
