using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface ICategoryQuery : IQuery<CategoryResponseDto>
{
    Task<IResult<List<Category>>> GetManyById(List<int> categoriesId);
}
