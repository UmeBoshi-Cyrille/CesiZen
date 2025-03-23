using Bogus;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Test.Fakers;

internal class CategoryFaker
{
    public static Faker<CategoryRequestDto> FakeCategoryRequestDtoGenerator(int id = 0)
    {
        return new Faker<CategoryRequestDto>()
            .RuleFor(a => a.Id, f => id != 0 ? f.Random.Int(1, 200) : id)
            .RuleFor(a => a.Name, f => f.Name.JobTitle());
    }

    public static Faker<Category> FakeCategoryGenerator(int id = 0)
    {
        return new Faker<Category>()
            .RuleFor(a => a.Id, f => id != 0 ? f.Random.Int(1, 200) : id)
            .RuleFor(a => a.Name, f => f.Name.JobTitle());
    }

    public static Faker<CategoryDto> FakeCategoryDtoGenerator()
    {
        return new Faker<CategoryDto>()
            .RuleFor(a => a.Id, f => f.Random.Int(1, 200))
            .RuleFor(a => a.Name, f => f.Name.JobTitle());
    }
}
