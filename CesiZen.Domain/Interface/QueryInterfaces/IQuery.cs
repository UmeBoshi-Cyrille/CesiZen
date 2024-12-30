namespace CesiZen.Domain.Interface;

public interface IQuery<T>
{
    T GetOne(int id);

    IEnumerable<T> GetAll();
}
