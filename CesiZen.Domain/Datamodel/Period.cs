namespace CesiZen.Domain.Datamodel;

public class Period : AId
{
    public DateTime BeginDate { get; set; }

    public DateTime EndDate { get; set; }

    public ICollection<Tracker>? Trackers { get; set; }
}
