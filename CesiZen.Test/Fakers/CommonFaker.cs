using Bogus;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
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

    public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
    {
        var mockDbSet = data.AsQueryable().BuildMockDbSet();
        //var mockDbSet = new Mock<DbSet<T>>();

        //mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        //mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        //mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        //mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

        return mockDbSet;
    }
}
