using CesiZen.Domain.Interfaces;
//using IResult = CesiZen.Domain.Interface.IResult;

namespace CesiZen.Domain.BusinessResult;

public class Result : IResult
{
    protected Error error;
    protected Info info;
    protected string exceptionMessage;
    protected string identifier;

    public Error Error => error != Error.None ? error : Error.None;
    public Info Info => info != Info.None ? info : Info.None;

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Result()
    {
        IsSuccess = true;
        error = Error.None;
        info = Info.None;
        exceptionMessage = string.Empty;
        identifier = string.Empty;
    }

    public Result(Info info)
    {
        IsSuccess = true;
        this.info = info;
        error = Error.None;
        exceptionMessage = string.Empty;
        identifier = string.Empty;
    }

    public Result(Error error)
    {
        IsSuccess = false;
        this.error = error;
        info = Info.None;
        exceptionMessage = string.Empty;
        identifier = string.Empty;
    }

    public Result(Error error, string exception)
    {
        IsSuccess = false;
        this.error = error;
        exceptionMessage = exception;
        info = Info.None;
        identifier = string.Empty;
    }

    public Result(Error error, string identifier, string exception)
    {
        IsSuccess = false;
        this.error = error;
        exceptionMessage = exception;
        this.identifier = identifier;
        info = Info.None;
    }

    /// <summary>
    /// Create a new Success Result
    /// </summary>
    /// <returns>A new Empty Success Result</returns>
    public static Result Success() => new();

    /// <summary>
    /// Create a new Success Result with an Info.
    /// </summary>
    /// <param name="info">The first value</param>
    /// <returns>A new Success Result with an Info</returns>
    public static Result Success(Info info) => new(info);

    /// <summary>
    /// return a new Result with an Error.
    /// </summary>
    /// <param name="error">The first value</param>
    /// <returns>a new Failure Result with an Error</returns>
    public static Result Failure(Error error) => new(error);

    /// <summary>
    /// return a new Result with an Error.
    /// </summary>
    /// <param name="error">The first value</param>
    /// <returns>a new Failure Result with an Error</returns>
    public static Result Failure(Error error, string exception) => new(error, exception);

    /// <summary>
    /// return a new Result with an Error.
    /// </summary>
    /// <param name="error">The first value</param>
    /// <returns>a new Failure Result with an Error</returns>
    public static Result Failure(Error error, string identifier, string exception) => new(error, identifier, exception);
}

public class Result<TValue> : Result, IResult<TValue>
{
    private readonly TValue value;

    public TValue Value => value;

    private Result(TValue value) : base()
    {
        this.value = value;
    }

    private Result(TValue value, Info info) : base(info)
    {
        this.value = value;
    }

    private Result(Error error) : base(error)
    {
        this.value = default!;
    }

    private Result(Error error, string exception) : base(error, exception)
    {
        this.value = default!;
    }

    private Result(Error error, string identifier, string exception) : base(error, identifier, exception)
    {
        this.value = default!;
    }

    /// <summary>
    /// Create a new Success Result with a value
    /// </summary>
    /// <param name="value"></param>
    /// <returns>A new Success Result with the value</returns>
    public static Result<TValue> Success(TValue value) => new(value);

    /// <summary>
    /// Create a new Success Result with a value and an Info. 
    /// </summary>
    /// <param name="value">The first value</param>
    /// <param name="info">The second value</param>
    /// <returns>A new Success Result with the value and the info</returns>
    public static Result<TValue> Success(TValue value, Info info) => new(value, info);

    /// <summary>
    /// Create a new Failure Result with an Error.
    /// </summary>
    /// <param name="error">The first value</param>
    /// <returns>a new Failure Result with an Error</returns>
    public static new Result<TValue> Failure(Error error) => new(error);

    /// <summary>
    /// Create a new Failure Result with an Error.
    /// </summary>
    /// <param name="error">The first value</param>
    /// <returns>a new Failure Result with an Error</returns>
    public static new Result<TValue> Failure(Error error, string exception) => new(error, exception);

    /// <summary>
    /// return a new Result with an Error.
    /// </summary>
    /// <param name="error">The first value</param>
    /// <returns>a new Failure Result with an Error</returns>
    public static new Result<TValue> Failure(Error error, string identifier, string exception) => new(error, identifier, exception);
}
