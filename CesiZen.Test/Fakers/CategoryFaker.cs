using Bogus;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Test.Fakers;

internal class CategoryFaker
{
    public static Faker<CategoryRequestDto> FakeCategoryRequestDtoGenerator(string id = "")
    {
        return new Faker<CategoryRequestDto>()
            .RuleFor(a => a.Id, f => string.IsNullOrEmpty(id) ? f.Random.Guid().ToString() : id)
            .RuleFor(a => a.Name, f => f.Name.JobTitle());
    }

    public static Faker<Category> FakeCategoryGenerator(string id = "")
    {
        return new Faker<Category>()
            .RuleFor(a => a.Id, f => string.IsNullOrEmpty(id) ? f.Random.Guid().ToString() : id)
            .RuleFor(a => a.Name, f => f.Name.JobTitle());
    }

    public static Faker<CategoryDto> FakeCategoryDtoGenerator()
    {
        return new Faker<CategoryDto>()
            .RuleFor(a => a.Id, f => f.Random.Guid().ToString())
            .RuleFor(a => a.Name, f => f.Name.JobTitle());
    }
}
