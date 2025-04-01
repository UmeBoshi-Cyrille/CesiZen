namespace CesiZen.Domain.Interfaces;

public interface ICommand<E, T>
{
    Task<IResult<E>> Insert(T entity);

    Task<IResult> Update(T entity);

    Task<IResult> Delete(int id);
}
