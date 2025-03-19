using CesiZen.Domain.Interfaces;

namespace CesiZen.Domain.BusinessResult;

public static class ResultExtension
{
    public static T Match<T>(
    this IResult result,
    Func<T> success,
    Func<Error, T> failure)
    {
        return result.IsSuccess ? success() : failure(result.Error!);
    }

    public static T Match<T, TValue>(
        this IResult<TValue> result,
        Func<TValue, T> success,
        Func<Error, T> failure)
    {
        return result.IsSuccess ? success(result.Value) : failure(result.Error!);
    }
}
