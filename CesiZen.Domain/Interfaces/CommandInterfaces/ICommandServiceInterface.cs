namespace CesiZen.Domain.Interfaces;

public interface ICommandServiceInterface<T>
{
    Task<IResult> Update(T entity);

    Task<IResult> Delete(string id);
}
