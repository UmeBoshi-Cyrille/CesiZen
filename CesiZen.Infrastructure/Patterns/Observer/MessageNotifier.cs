using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;

namespace CesiZen.Infrastructure;

public class MessageNotifier : INotifier
{
    public event EventHandler<MessageEventArgs>? MessageEvent;

    public void NotifyObservers(MessageEventArgs dto)
    {
        MessageEvent?.Invoke(this, dto);
    }
}
