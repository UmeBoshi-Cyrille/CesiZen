namespace CesiZen.Domain.Interface;

public interface ICommand<T>
{
    Task<IResult> Insert(T entity);

    Task<IResult> Update(T entity);

    Task<IResult> Delete(int id);
}
