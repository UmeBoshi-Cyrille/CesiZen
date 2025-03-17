using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interface;

public interface IQuery<T>
{
    Task<IResult<T>> GetByIdAsync(int id);

    Task<IResult<PagedResult<T>>> GetAllAsync(int pageNumber, int pageSize);
}
