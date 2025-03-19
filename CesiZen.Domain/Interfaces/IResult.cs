using CesiZen.Domain.BusinessResult;

namespace CesiZen.Domain.Interfaces;

public interface IResult
{
    bool IsSuccess { get; }
    bool IsFailure { get; }

    Error Error { get; }
    Info Info { get; }
}

public interface IResult<out TValue> : IResult
{
    TValue Value { get; }
}