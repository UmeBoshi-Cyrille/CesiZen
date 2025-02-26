namespace CesiZen.Domain.Interface;

public interface ICommand<T>
{
    void Insert(T entity);

    void Update(T entity);

    void Delete(int id);
}
