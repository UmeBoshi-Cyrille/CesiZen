using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IQuery<T, E>
{
    Task<IResult<T>> GetByIdAsync(int id);

    Task<IResult<PagedResultDto<E>>> GetAllAsync(int pageNumber, int pageSize);
}
