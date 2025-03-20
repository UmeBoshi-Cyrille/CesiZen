namespace CesiZen.Domain.DataTransfertObject;

public class MessageEventDto : EventArgs
{
    public string Email { get; set; }
    public string Body { get; set; }
    public string Subject { get; set; }
}
