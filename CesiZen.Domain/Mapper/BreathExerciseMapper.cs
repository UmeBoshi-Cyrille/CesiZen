using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Mapper;

public static class BreathExerciseMapper
{
    public static BreathExercise Map(this NewBreathExerciseDto dto)
    {
        return new BreathExercise
        {
            Title = dto.Title,
            Time = dto.Time,
            ExerciseType = dto.ExerciseType,
            UserId = dto.userId,
            EditedAt = DateTime.UtcNow,
        };
    }

    public static BreathExercise Map(this BreathExerciseDto dto)
    {
        return new BreathExercise
        {
            Id = dto.Id,
            Title = dto.Title,
            Time = dto.Time,
            ExerciseType = dto.ExerciseType,
            UserId = dto.userId,
            EditedAt = DateTime.UtcNow,
        };
    }

    public static BreathExerciseDto Map(this BreathExercise model)
    {
        return new BreathExerciseDto
        {
            Id = model.Id,
            Title = model.Title,
            Time = model.Time,
            ExerciseType = model.ExerciseType,
            EditedAt = model.EditedAt,
        };
    }

    public static List<BreathExerciseDto> Map(this List<BreathExercise> model)
    {
        List<BreathExerciseDto> dto = new();

        for (var i = 0; i < model.Count; i++)
        {
            var item = model[i].Map();
            dto.Add(item);
        }

        return dto;
    }

    public static List<BreathExercise> Map(this List<NewBreathExerciseDto> dto)
    {
        List<BreathExercise> model = new();

        for (var i = 0; i < dto.Count; i++)
        {
            var item = dto[i].Map();
            model.Add(item);
        }

        return model;
    }

    public static List<BreathExercise> Map(this List<BreathExerciseDto> dto)
    {
        List<BreathExercise> model = new();

        for (var i = 0; i < dto.Count; i++)
        {
            var item = dto[i].Map();
            model.Add(item);
        }

        return model;
    }
}
