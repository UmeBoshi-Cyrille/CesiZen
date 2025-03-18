using Serilog;

namespace CesiZen.Domain.BusinessResult;

public static class ErrorService
{
    #region ISNULL Methods - String
    /// <summary>
    /// Check whether the string is null, Empty or got whiteSpace.
    /// </summary>
    /// <param name="argument">argument</param>
    /// <returns>true or false</returns>
    public static bool IsNullString(this string argument)
       => !string.IsNullOrEmpty(argument) && !string.IsNullOrWhiteSpace(argument);

    /// <summary>
    /// Check whether the string is null, Empty or got whiteSpace.
    /// Log an error or return value, depending on the Result.
    /// </summary>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<string> IsNull(
        this string argument,
        ILogger logger)
    {
        return IsNullStringValue(argument, logger);
    }

    /// <summary>
    /// Check whether the string is null, Empty or got whiteSpace.
    /// Log an error or return value and log Success info, depending on the Result.
    /// </summary>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="info">Fifth argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<string> IsNull(
        this string argument,
        ILogger logger,
        Info info)
    {
        return IsNullStringValue(argument, logger, info);
    }

    /// <summary>
    /// Check whether the string is null, Empty or got whiteSpace.
    /// Log an error and throw exception or return value, depending on the Result.
    /// </summary>
    /// <typeparam name="E"></typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<string> IsNull<E>(
        this string argument,
        ILogger logger) where E : Exception, new()
    {
        return IsNullStringValue<E>(argument, logger);
    }

    /// <summary>
    /// Check whether the string is null, Empty or got whiteSpace.
    /// Log an error and throw exception or return value and log Success info, depending on the Result.
    /// </summary>
    /// <typeparam name="E"></typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="info">Fifth argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<string> IsNull<E>(
        this string argument,
        ILogger logger,
        Info info) where E : Exception, new()
    {
        return IsNullStringValue<E>(argument, logger, info);
    }

    private static Result<string> IsNullStringValue(
        string argument,
        ILogger logger,
        Info? info = null)
    {
        if (string.IsNullOrEmpty(argument))
            return Result<string>.Failure(Error.NullOrEmpty(""), logger);

        if (string.IsNullOrWhiteSpace(argument))
            return Result<string>.Failure(Error.NullOrWhiteSpace(""), logger);


        if (info != null)
            return Result<string>.Success(argument, logger, info);

        return Result<string>.Success(argument);
    }

    private static Result<string> IsNullStringValue<E>(
        string argument,
        ILogger logger,
        Info? info = null) where E : Exception, new()
    {
        if (string.IsNullOrEmpty(argument))
            return Result<string>.Failure<E>(Error.NullValue(""), logger);

        if (string.IsNullOrWhiteSpace(argument))
            return Result<string>.Failure<E>(Error.NullOrWhiteSpace(""), logger);

        if (info != null)
            return Result<string>.Success(argument, logger, info);

        return Result<string>.Success(argument);
    }
    #endregion

    #region ISNULL Methods - with T Argument
    /// <summary>
    /// Check whether the value is null or not.
    /// </summary>
    /// <typeparam name="T">Generic type argument</typeparam>
    /// <param name="argument">First argument</param>
    /// <returns>true or false</returns>
    public static bool IsNullArgument<T>(this T argument) where T : class
        => argument != null;

    /// <summary>
    /// Check whether the value is null or not.
    /// </summary>
    /// <typeparam name="T">Generic type argument</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="error">Third argument</param>
    /// <param name="info">Fourth argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value and the info</returns>
    public static Result<T> IsNull<T>(
        this T argument,
        Error? error = null,
        Info? info = null)
    {
        return IsNullValue(argument, error, info);
    }

    /// <summary>
    /// Check whether the value is null or not.
    /// Depending on the Result: Log an Error or simply return the value.
    /// </summary>
    /// <typeparam name="T">Generic type argument</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<T> IsNull<T>(
        this T argument,
        ILogger logger)
    {
        return IsNullValue(argument, logger);
    }

    /// <summary>
    /// Check whether the value is null or not.
    /// Depending on the Result: Log an Error or simply return the value.
    /// </summary>
    /// <typeparam name="T">Generic type argument</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="error">Third argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<T> IsNull<T>(
        this T argument,
        ILogger logger,
        Error error)
    {
        return IsNullValue(argument, logger, error);
    }

    /// <summary>
    /// Check whether the value is null or not.
    /// Depending on the Result: Log an Error or simply return the value and log the Success.
    /// </summary>
    /// <typeparam name="T">Generic type argument</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="info">Fourth argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<T> IsNull<T>(
        this T argument,
        ILogger logger,
        Info info)
    {
        return IsNullValue(argument, logger, null, info);
    }

    /// <summary>
    /// Check whether the value is null or not.
    /// Depending on the Result: Log an Error or simply return the value and log the Success.
    /// </summary>
    /// <typeparam name="T">Generic type argument</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="error">Third argument</param>
    /// <param name="info">Fourth argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<T> IsNull<T>(
        this T argument,
        ILogger logger,
        Error error,
        Info info)
    {
        return IsNullValue(argument, logger, error, info);
    }

    /// <summary>
    /// Check whether the value is null or not.
    /// Depending on the Result and the arguments: Log an Error and send an Exception or simply return the value. 
    /// </summary>
    /// <typeparam name="T">Generic type argument</typeparam>
    /// <typeparam name="E">Generic Exception type</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<T> IsNull<T, E>(
        this T argument,
        ILogger logger) where E : Exception, new()
    {
        return IsNullValue<T, E>(argument, logger);
    }

    /// <summary>
    /// Check whether the value is null or not.
    /// Depending on the Result and the arguments: Log an Error and send an Exception or simply return the value and log the Success. 
    /// </summary>
    /// <typeparam name="T">Generic type argument</typeparam>
    /// <typeparam name="E">Generic Exception type</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="info">Fourth argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<T> IsNull<T, E>(
        this T argument,
        ILogger logger,
        Info info) where E : Exception, new()
    {
        return IsNullValue<T, E>(argument, logger, null, info);
    }

    /// <summary>
    /// Check whether the value is null or not.
    /// Depending on the Result and the arguments: Log an Error and send an Exception or simply return the value. 
    /// </summary>
    /// <typeparam name="T">Generic type argument</typeparam>
    /// <typeparam name="E">Generic Exception type</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="error">Third argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<T> IsNull<T, E>(
        this T argument,
        ILogger logger,
        Error error) where E : Exception, new()
    {
        return IsNullValue<T, E>(argument, logger, error);
    }

    /// <summary>
    /// Check whether the value is null or not.
    /// Depending on the Result and the arguments: Log an Error and send an Exception or simply return the value and log the Success. 
    /// </summary>
    /// <typeparam name="T">Generic type argument</typeparam>
    /// <typeparam name="E">Generic Exception type</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="error">Third argument</param>
    /// <param name="info">Fourth argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<T> IsNull<T, E>(
        this T argument,
        ILogger logger,
        Error error,
        Info info) where E : Exception, new()
    {
        return IsNullValue<T, E>(argument, logger, error, info);
    }

    private static Result<T> IsNullValue<T>(
        this T argument,
        Error? error = null,
        Info? info = null)
    {
        if (argument == null)
        {
            if (error == null)
                return Result<T>.Failure(Error.NullValue(""));

            return Result<T>.Failure(error);
        }

        if (info != null)
            return Result<T>.Success(argument, info);

        return Result<T>.Success(argument);
    }

    private static Result<T> IsNullValue<T>(
        T argument,
        ILogger logger,
        Error? error = null,
        Info? info = null)
    {
        if (argument == null)
        {
            if (error == null)
                return Result<T>.Failure(Error.NullValue(""), logger);

            return Result<T>.Failure(error, logger);
        }

        if (info != null)
            return Result<T>.Success(argument, logger, info);

        return Result<T>.Success(argument);
    }

    private static Result<T> IsNullValue<T, E>(
        T argument,
        ILogger logger,
        Error? error = null,
        Info? info = null) where E : Exception, new()
    {
        if (argument == null)
        {
            if (error == null)
                return Result<T>.Failure<E>(Error.NullValue(""), logger);

            return Result<T>.Failure<E>(error, logger);
        }

        if (info != null)
            return Result<T>.Success(argument, logger, info);

        return Result<T>.Success(argument);
    }
    #endregion

    #region ANY Methods - with List or IEnumerable T argument
    ///// <summary>
    ///// Check List content and return the proper Result type. Log the Error if no content.
    ///// </summary>
    ///// <typeparam name="T">Generic type of List</typeparam>
    ///// <param name="argument">First argument</param>
    ///// <param name="logger">Second argument</param>
    ///// <param name="error">Third argument</param>
    ///// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    //public static Result<List<T>> HasAny<T>(
    //    this List<T> argument,
    //    ILogger logger,
    //    Error error)
    //{
    //    if (!argument.Any())
    //    {
    //        // TODO : Error message
    //        return Result<List<T>>.Failure(error, logger);
    //    }

    //    return Result<List<T>>.Success(argument);
    //}

    ///// <summary>
    ///// Check List content and return the proper Result type.
    ///// Depending on the Result and arguments: Log an Error or simply return the value and log the Success.
    ///// </summary>
    ///// <typeparam name="T">Generic value type of List</typeparam>
    ///// <param name="argument">First argument</param>
    ///// <param name="logger">Second argument</param>
    ///// <param name="error">Third argument</param>
    ///// <param name="info">Fourth argument</param>
    ///// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    //public static Result<List<T>> HasAny<T>(
    //    this List<T> argument,
    //    ILogger logger,
    //    Error? error = null,
    //    Info? info = null)
    //{
    //    if (!argument.Any())
    //{
    //    if (error == null)
    //        return Result<List<T>>.Failure(Error.EmptyValue(argument, ""), logger);

    //    return Result<List<T>>.Failure(error, logger);
    //}

    //if (info != null)
    //    return Result<List<T>>.Success(argument, logger, info);

    //return Result<List<T>>.Success(argument);
    //}

    ///// <summary>
    ///// Check List content and return the proper Result type.
    ///// Depending on the Result and arguments: Log an Error and send an Exception or simply return the value and log the Success 
    ///// </summary>
    ///// <typeparam name="T">Generic value type of List</typeparam>
    ///// <typeparam name="E">Generic Exception type</typeparam>
    ///// <param name="argument">First argument</param>
    ///// <param name="logger">Second argument</param>
    ///// <param name="error">Third argument</param>
    ///// <param name="info">Fourth argument</param>
    ///// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    //public static Result<List<T>> HasAny<T, E>(
    //    this List<T> argument,
    //    ILogger logger,
    //    Error? error = null,
    //    Info? info = null) where E : Exception, new()
    //{
    //    if (!argument.Any())
    //{
    //    if (error == null)
    //        return Result<List<T>>.Failure<E>(Error.EmptyValue(argument, ""), logger);

    //    return Result<List<T>>.Failure<E>(error, logger);
    //}

    //if (info != null)
    //    return Result<List<T>>.Success(argument, logger, info);

    //return Result<List<T>>.Success(argument);
    //}

    /// <summary>
    /// Check IEnumerable's content and return the proper Result type, log an Error or return the value depending on the Result.
    /// </summary>
    /// <typeparam name="T">Generic type of IEnumerable</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<IEnumerable<T>> HasAny<T>(
        this IEnumerable<T> argument,
        ILogger logger)
    {
        return HasAnyValue(argument, logger);
    }

    /// <summary>
    /// Check IEnumerable's content and return the proper Result type, log an Error or the information depending on the Result.
    /// </summary>
    /// <typeparam name="T">Generic type of IEnumerable</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="info">Fourth argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<IEnumerable<T>> HasAny<T>(
        this IEnumerable<T> argument,
        ILogger logger,
        Info info)
    {
        return HasAnyValue(argument, logger, null, info);
    }

    /// <summary>
    /// Check IEnumerable's content and return the proper Result type, log an Error or return the value depending on the Result.
    /// </summary>
    /// <typeparam name="T">Generic type of IEnumerable</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="error">Third argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<IEnumerable<T>> HasAny<T>(
        this IEnumerable<T> argument,
        ILogger logger,
        Error error)
    {
        return HasAnyValue(argument, logger, error);
    }

    /// <summary>
    /// Check IEnumerable's content and return the proper Result type, log an Error or the information depending on the Result.
    /// </summary>
    /// <typeparam name="T">Generic type of IEnumerable</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="error">Third argument</param>
    /// <param name="info">Fourth argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<IEnumerable<T>> HasAny<T>(
        this IEnumerable<T> argument,
        ILogger logger,
        Error error,
        Info info)
    {
        return HasAnyValue(argument, logger, error, info);
    }

    /// <summary>
    /// Check IEnumerable's content and return the proper Result type.
    /// Depending on the Result and arguments: Log an Error and send an Exception or return the value and Log the Success Info
    /// </summary>
    /// <typeparam name="T">Generic value type of IEnumerable</typeparam>
    /// <typeparam name="E">Generic Exception type</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<IEnumerable<T>> HasAny<T, E>(
        this IEnumerable<T> argument,
        ILogger logger) where E : Exception, new()
    {
        return HasAnyValue<T, E>(argument, logger);
    }

    /// <summary>
    /// Check IEnumerable's content and return the proper Result type.
    /// Depending on the Result: Log an Error and send an Exception or return the value and Log the Success Info
    /// </summary>
    /// <typeparam name="T">Generic value type of IEnumerable</typeparam>
    /// <typeparam name="E">Generic Exception type</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="info">Fourth argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<IEnumerable<T>> HasAny<T, E>(
        this IEnumerable<T> argument,
        ILogger logger,
        Info info) where E : Exception, new()
    {
        return HasAnyValue<T, E>(argument, logger, null, info);
    }

    /// <summary>
    /// Check IEnumerable's content and return the proper Result type.
    /// Depending on the Result: Log an Error and send an Exception or return the value and Log a Success Info
    /// </summary>
    /// <typeparam name="T">Generic value type of IEnumerable</typeparam>
    /// <typeparam name="E">Generic Exception type</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="error">Third argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<IEnumerable<T>> HasAny<T, E>(
        this IEnumerable<T> argument,
        ILogger logger,
        Error error) where E : Exception, new()
    {
        return HasAnyValue<T, E>(argument, logger, error);
    }

    /// <summary>
    /// Check IEnumerable's content and return the proper Result type.
    /// Depending on the Result: Log an Error and send an Exception or return the value and Log the Success Info
    /// </summary>
    /// <typeparam name="T">Generic value type of IEnumerable</typeparam>
    /// <typeparam name="E">Generic Exception type</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="error">Third argument</param>
    /// <param name="info">Fourth argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<IEnumerable<T>> HasAny<T, E>(
        this IEnumerable<T> argument,
        ILogger logger,
        Error error,
        Info info) where E : Exception, new()
    {
        return HasAnyValue<T, E>(argument, logger, error, info);
    }

    private static Result<IEnumerable<T>> HasAnyValue<T>(
        IEnumerable<T> argument,
        ILogger logger,
        Error? error = null,
        Info? info = null)
    {
        if (!argument.Any())
        {
            if (error == null)
                return Result<IEnumerable<T>>.Failure(Error.NullValue(""), logger);

            return Result<IEnumerable<T>>.Failure(error, logger);
        }

        if (info != null)
            return Result<IEnumerable<T>>.Success(argument, logger, info);

        return Result<IEnumerable<T>>.Success(argument);
    }

    private static Result<IEnumerable<T>> HasAnyValue<T, E>(
        IEnumerable<T> argument,
        ILogger logger,
        Error? error = null,
        Info? info = null) where E : Exception, new()
    {
        if (!argument.Any())
        {
            if (error == null)
                return Result<IEnumerable<T>>.Failure<E>(Error.NullValue(""), logger);

            return Result<IEnumerable<T>>.Failure<E>(error, logger);
        }

        if (info != null)
            return Result<IEnumerable<T>>.Success(argument, logger, info);

        return Result<IEnumerable<T>>.Success(argument);
    }
    #endregion
}

