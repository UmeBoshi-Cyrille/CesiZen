using Bogus;
using CesiZen.Domain.Datamodel;

namespace CesiZen.Infrastructure.Seeders;

internal static class ImageSeeder
{
    internal static Faker<Image> FakeGenerator()
    {
        return new Faker<Image>()
            .UseSeed(42)
            .RuleFor(i => i.Alternative, f => GenerateAlternativeText(f))
            .RuleFor(i => i.Title, f => GenerateTitle(f))
            .RuleFor(i => i.Path, f => GeneratePath(f));
    }

    private static string GenerateAlternativeText(Faker faker)
    {
        var subjects = new[] { "a woman", "a forest", "a city", "a lake", "a mountain", "a man", "a child", "a beach" };
        var actions = new[] { "at sunset", "in the rain", "in the morning", "in winter", "in summer", "under a clear sky", "with vibrant color" };

        return $"{faker.PickRandom(subjects)} {faker.PickRandom(actions)}";
    }

    private static string GenerateTitle(Faker faker)
    {
        var adjectives = new[] { "Beautiful", "Relaxing", "Stunning", "Vibrant", "Serene", "BreathTaking" };
        var subjects = new[] { "Meal", "Sunset", "Picture", "Landscape", "Scene", "Example", "Nature", "Forest", "Tree" };

        return $"{faker.PickRandom(adjectives)} {faker.PickRandom(subjects)}";
    }

    public static string GeneratePath(Faker faker)
    {
        var subjects = new[] { "lion", "tree", "woman", "beach", "forest", "mountain", "meal", "fish", "something", "man", "sun", "mammouth", "kitty" };
        var extensions = new[] { "png", "jpg" };

        return $"{faker.PickRandom(subjects)}.{faker.PickRandom(extensions)}";
    }
}
