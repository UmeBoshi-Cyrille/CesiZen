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
            .RuleFor(a => a.Id, f => f.Random.Int(1, 200))
            .RuleFor(a => a.Title, f => f.Name.JobTitle())
            .RuleFor(a => a.Time, f => f.Random.Int(1, 600))
            .RuleFor(a => a.ExerciseType, f => f.PickRandom<ExerciceType>());
    }

    public static Faker<BreathExerciseMinimumDto> FakeBreathExerciseMinimumDtoGenerator()
    {
        return new Faker<BreathExerciseMinimumDto>()
            .RuleFor(a => a.Id, f => f.Random.Int(1, 200))
            .RuleFor(a => a.Title, f => f.Name.JobTitle())
            .RuleFor(a => a.ExerciseType, f => f.PickRandom<ExerciceType>());
    }

    public static Faker<NewBreathExerciseDto> FakeNewBreathExerciseDtoGenerator()
    {
        return new Faker<NewBreathExerciseDto>()
            .RuleFor(a => a.Title, f => f.Name.JobTitle())
            .RuleFor(a => a.Time, f => f.Random.Int(1, 600))
            .RuleFor(a => a.ExerciseType, f => f.Random.Int(1, 3));
    }

    public static Faker<BreathExercise> FakeBreathExerciseGenerator(int userId = 0)
    {
        return new Faker<BreathExercise>()
            .RuleFor(a => a.Id, f => f.Random.Int(1, 200))
            .RuleFor(a => a.Title, f => f.Name.JobTitle())
            .RuleFor(a => a.Time, f => f.Random.Int(1, 600))
            .RuleFor(a => a.ExerciseType, f => f.PickRandom<ExerciceType>())
            .RuleFor(a => a.UserId, f => userId != 0 ? f.Random.Int(1, 200) : userId);
    }
}
