namespace CesiZen.Domain.Interfaces;

public interface ICommand<T>
{
    Task<IResult> Insert(T entity);

    Task<IResult> Update(T entity);

    Task<IResult> Delete(int Id);
}
