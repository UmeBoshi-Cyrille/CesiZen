using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IQuery<T>
{
    Task<IResult<T>> GetByIdAsync(int id);

    Task<IResult<PagedResult<T>>> GetAllAsync(int pageNumber, int pageSize);
}
