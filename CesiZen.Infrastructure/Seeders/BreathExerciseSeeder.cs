using Bogus;
using CesiZen.Domain.Datamodel;

namespace CesiZen.Infrastructure.Seeders;

internal static class BreathExerciseSeeder
{
    internal static Faker<BreathExercise> FakeGenerator()
    {
        return new Faker<BreathExercise>()
            .UseSeed(42)
            .RuleFor(i => i.EditedAt, f => f.Date.Past(2).ToUniversalTime())
            .RuleFor(i => i.Time, f => f.Random.Int(1, 10))
            .RuleFor(i => i.Title, f => GenerateTitle(f));
    }

    private static string GenerateTitle(Faker faker)
    {
        var titles = new[]
        {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K",
            "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V",
            "X", "Y", "Z",
        };

        return $"{faker.PickRandom(titles)}";
    }
}
