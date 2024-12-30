namespace CesiZen.Domain.Datamodel;

public class Diagnostic : AId
{
    public ICollection<DiagnosticEvent>? Events { get; set; }

    public ICollection<DiagnosticResult>? Results { get; set; }
}
