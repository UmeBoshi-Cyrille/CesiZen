using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IObserver
{
    void Update(object sender, MessageEventDto dto);
}
