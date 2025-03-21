namespace CesiZen.Domain.DataTransfertObject;

public class MessageEventArgs : EventArgs
{
    public string Email { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
}
