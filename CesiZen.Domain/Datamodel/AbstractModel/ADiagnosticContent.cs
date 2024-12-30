namespace CesiZen.Domain.Datamodel;

public class ADiagnosticContent : ATitle
{
    public string Content { get; set; } = string.Empty;

    public ushort Score { get; set; }
}
