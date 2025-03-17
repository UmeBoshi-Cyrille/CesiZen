using Bogus;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Infrastructure.DatabaseContext;
using Moq;

namespace CesiZen.Test.Fakers;

internal class CommonFaker
{
    public static Faker<PageParameters> FakePageParametersGenerator()
    {
        return new Faker<PageParameters>()
            .RuleFor(a => a.PageNumber, f => f.Random.Int(1, 200))
            .RuleFor(a => a.PageSize, f => f.Random.Int(1, 200));
    }

    public static Mock<MongoDbContext> GenerateContext()
    {
        return new Mock<MongoDbContext>();
    }
}
