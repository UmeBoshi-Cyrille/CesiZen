using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface INotifier
{
    event EventHandler<MessageEventArgs> MessageEvent;

    void NotifyObservers(MessageEventArgs dto);
}
