using Bogus;
using CesiZen.Domain.Datamodel;

namespace CesiZen.Infrastructure.Seeders;

internal static class CategorySeeder
{
    internal static Faker<Category> FakeGenerator()
    {
        return new Faker<Category>()
            .UseSeed(42)
            .RuleFor(i => i.Name, f => f.Commerce.Categories(1).First());
    }
}
