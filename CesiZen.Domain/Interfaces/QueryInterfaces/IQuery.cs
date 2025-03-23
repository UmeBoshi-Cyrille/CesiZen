using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IQuery<T>
{
    Task<IResult<T>> GetByIdAsync(int Id);

    Task<IResult<PagedResultDto<T>>> GetAllAsync(int pageNumber, int pageSize);
}
