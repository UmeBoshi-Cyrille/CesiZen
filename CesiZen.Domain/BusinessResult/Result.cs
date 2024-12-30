using CesiZen.Domain.Interface;
using Serilog;
//using IResult = CesiZen.Domain.Interface.IResult;

namespace CesiZen.Domain.BusinessResult;

public class Result : IResult
{
    protected Error error;
    protected Info info;

    public Error Error => error != Error.None ? error : Error.None;
    public Info Info => info != Info.None ? info : Info.None;

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Result()
    {
        IsSuccess = true;
        error = Error.None;
    }

    public Result(Info info)
    {
        IsSuccess = true;
        this.info = info;
        error = Error.None;
    }

    public Result(Error error)
    {
        IsSuccess = false;
        this.error = error;
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
    /// Create a new Success Result which log the information. 
    /// </summary>
    /// <param name="info">The first value</param>
    /// <param name="logger">The second value</param>
    /// <returns>A new Success Result with an Info</returns>
    public static Result Success(Info info, ILogger logger)
    {
        logger.Information(InfoMessage(info));

        return Success(info);
    }

    /// <summary>
    /// return a new Result with an Error.
    /// </summary>
    /// <param name="error">The first value</param>
    /// <returns>a new Failure Result with an Error</returns>
    public static Result Failure(Error error) => new(error);

    /// <summary>
    /// Log an Error with it's type and message
    /// </summary>
    /// <param name="error">The first value</param>
    /// <param name="logger">The second value</param>
    /// <returns>a new Failure Result with an Error</returns>
    public static Result Failure(Error error, ILogger logger)
    {
        logger.Error(ErrorMessage(error));

        return Failure(error);
    }

    /// <summary>
    /// Log an Error with it's type and message and throw the choosen exception.
    /// </summary>
    /// <typeparam name="E">The Defined exception</typeparam>
    /// <param name="error">The first value</param>
    /// <param name="logger">The second value</param>
    /// <returns>throw the defined exception</returns>
    public static Result Failure<E>(
        Error error,
        ILogger logger) where E : Exception, new()
    {
        logger.Error(ErrorMessage(error));

        E exception = (E)Activator.CreateInstance(typeof(E), ErrorMessage(error));

        throw exception;
    }

    protected static string ErrorMessage(Error error)
    {
        return $"[ERROR][Type: {error.Type}] -Message: {error.Message}";
    }

    protected static string InfoMessage(Info info)
    {
        return $"[INFO][Type: {info.Type}] -Message: {info.Message}";
    }
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
    /// Create a new Success Result which log the information. 
    /// </summary>
    /// <param name="value">The first value</param>
    /// <param name="info">The second value</param>
    /// <param name="logger">The Third value</param>
    /// <returns>A new Success Result with the value</returns>
    public static Result<TValue> Success(TValue value, ILogger logger, Info info)
    {
        logger.Information(InfoMessage(info));

        return new(value);
    }

    /// <summary>
    /// Create a new Failure Result with an Error.
    /// </summary>
    /// <param name="error">The first value</param>
    /// <returns>a new Failure Result with an Error</returns>
    public static new Result<TValue> Failure(Error error) => new(error);

    /// <summary>
    /// Log an Error with it's type and message
    /// </summary>
    /// <param name="error">The first value</param>
    /// <param name="logger">The second value</param>
    /// <returns>a new Failure Result with an Error</returns>
    public static new Result<TValue> Failure(Error error, ILogger logger)
    {
        logger.Error(ErrorMessage(error));

        return new(error);
    }

    /// <summary>
    /// Log an Error with it's type and message and throw the choosen exception.
    /// </summary>
    /// <typeparam name="E">The Defined exception</typeparam>
    /// <param name="error">The first value</param>
    /// <param name="logger">The second value</param>
    /// <returns>throw the defined exception</returns>
    public static new Result<TValue> Failure<E>(
        Error error,
        ILogger logger) where E : Exception, new()
    {
        logger.Error(ErrorMessage(error));

        E exception = (E)Activator.CreateInstance(typeof(E), ErrorMessage(error));

        throw exception;
    }
}
