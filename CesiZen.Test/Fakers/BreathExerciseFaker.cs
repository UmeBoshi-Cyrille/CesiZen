using Bogus;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Enum;

namespace CesiZen.Test.Fakers;

internal class BreathExerciseFaker
{
    public static Faker<BreathExerciseDto> FakeBreathExerciseDtoGenerator()
    {
        return new Faker<BreathExerciseDto>()
            .RuleFor(a => a.Id, f => f.Random.Guid().ToString())
            .RuleFor(a => a.Title, f => f.Name.JobTitle())
            .RuleFor(a => a.Time, f => f.Random.Int(1, 600))
            .RuleFor(a => a.ExerciseType, f => f.PickRandom<ExerciceType>());
    }

    public static Faker<BreathExercise> FakeBreathExerciseGenerator(string userId = "")
    {
        return new Faker<BreathExercise>()
            .RuleFor(a => a.Id, f => f.Random.Guid().ToString())
            .RuleFor(a => a.Title, f => f.Name.JobTitle())
            .RuleFor(a => a.Time, f => f.Random.Int(1, 600))
            .RuleFor(a => a.ExerciseType, f => f.PickRandom<ExerciceType>())
            .RuleFor(a => a.UserId, f => string.IsNullOrEmpty(userId) ? f.Random.Guid().ToString() : userId);

    }


}
