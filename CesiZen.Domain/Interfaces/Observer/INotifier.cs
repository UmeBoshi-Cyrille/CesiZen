using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface INotifier
{
    event EventHandler<MessageEventDto> MessageEvent;

    void NotifyObservers(MessageEventDto dto);
}
