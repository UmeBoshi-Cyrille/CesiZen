using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;

namespace CesiZen.Infrastructure;

public class MessageNotifier : INotifier
{
    public event EventHandler<MessageEventDto>? MessageEvent;

    public void NotifyObservers(MessageEventDto dto)
    {
        MessageEvent?.Invoke(this, dto);
    }
}
