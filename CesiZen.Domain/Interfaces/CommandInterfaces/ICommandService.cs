namespace CesiZen.Domain.Interfaces;

public interface ICommandService<T>
{
    Task<IResult> Update(T entity);

    Task<IResult> Delete(int id);
}
