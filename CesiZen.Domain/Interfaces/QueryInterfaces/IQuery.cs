namespace CesiZen.Domain.Interface;

public interface IQuery<T>
{
    Task<IResult<T>> GetByIdAsync(int id);

    Task<IResult<IEnumerable<T>>> GetAllAsync();
}
