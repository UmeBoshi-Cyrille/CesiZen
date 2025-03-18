namespace CesiZen.Domain.BusinessResult;

public static class ErrorService
{
    #region ISNULL Methods - String
    /// <summary>
    /// Check whether the string is nullmpty or got whiteSpace.
    /// </summary>
    /// <param name="argument">argument</param>
    /// <returns>true or false</returns>
    public static bool IsNullString(this string argument)
       => !string.IsNullOrEmpty(argument) && !string.IsNullOrWhiteSpace(argument);

    /// <summary>
    /// Check whether the string is nullmpty or got whiteSpace.
    /// Log an error or return value, depending on the Result.
    /// </summary>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<string> IsNull(
        this string argument)
    {
        return IsNullStringValue(argument);
    }

    /// <summary>
    /// Check whether the string is nullmpty or got whiteSpace.
    /// Log an error or return value and log Success info, depending on the Result.
    /// </summary>
    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="info">Fifth argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<string> IsNull(
        this string argument,
        Info info)
    {
        return IsNullStringValue(argument, info);
    }

    private static Result<string> IsNullStringValue(
        string argument,
        Info? info = null)
    {
        if (string.IsNullOrEmpty(argument))
            return Result<string>.Failure(Error.NullOrEmpty(""));

        if (string.IsNullOrWhiteSpace(argument))
            return Result<string>.Failure(Error.NullOrWhiteSpace(""));


        if (info != null)
            return Result<string>.Success(argument, info);

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
        this T argument)
    {
        return IsNullValue(argument);
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
        Error error)
    {
        return IsNullValue(argument, error);
    }

    /// <summary>
    /// Check whether the value is null or not.
    /// </summary>
    /// <typeparam name="T">Generic type argument</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="info">Fourth argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<T> IsNull<T>(
        this T argument,
        Info info)
    {
        return IsNullValue(argument, null, info);
    }

    /// <summary>
    /// Check whether the value is null or not. 
    /// </summary>
    /// <typeparam name="T">Generic type argument</typeparam>
    /// <param name="argument">First argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<T> IsNull<T>(
        this T argument)
    {
        return IsNullValue(argument);
    }

    /// <summary>
    /// Check whether the value is null or not.
    /// </summary>
    /// <typeparam name="T">Generic type argument</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="info">Fourth argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<T> IsNull<T>(
        this T argument,
        Info info)
    {
        return IsNullValue(argument, null, info);
    }

    /// <summary>
    /// Check whether the value is null or not. 
    /// </summary>
    /// <typeparam name="T">Generic type argument</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="error">Third argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<T> IsNull<T>(
        this T argument,
        Error error)
    {
        return IsNullValue(argument, error);
    }

    /// <summary>
    /// Check whether the value is null or not.
    /// </summary>
    /// <typeparam name="T">Generic type argument</typeparam>
    /// <param name="argument">First argument</param>
    /// <param name="error">Third argument</param>
    /// <param name="info">Fourth argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<T> IsNull<T>(
        this T argument,
        Error error,
        Info info)
    {
        return IsNullValue(argument, error, info);
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
    //    
    //    Error error)
    //{
    //    if (!argument.Any())
    //    {
    //        // TODO : Error message
    //        return Result<List<T>>.Failure(error);
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
    //    
    //    Error? error = null,
    //    Info? info = null)
    //{
    //    if (!argument.Any())
    //{
    //    if (error == null)
    //        return Result<List<T>>.Failure(Error.EmptyValue(argument, ""));

    //    return Result<List<T>>.Failure(error);
    //}

    //if (info != null)
    //    return Result<List<T>>.Success(argument, info);

    //return Result<List<T>>.Success(argument);
    //}

    ///// <summary>
    ///// Check List content and return the proper Result type.
    ///// Depending on the Result and arguments: Log an Error and send an Exception or simply return the value and log the Success 
    ///// </summary>
    ///// <typeparam name="T">Generic value type of List</typeparam>
    //
    ///// <param name="argument">First argument</param>
    ///// <param name="logger">Second argument</param>
    ///// <param name="error">Third argument</param>
    ///// <param name="info">Fourth argument</param>
    ///// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    //public static Result<List<T>> HasAny<T>(
    //    this List<T> argument,
    //    
    //    Error? error = null,
    //    Info? info = null)
    //{
    //    if (!argument.Any())
    //{
    //    if (error == null)
    //        return Result<List<T>>.Failure<E>(Error.EmptyValue(argument, ""));

    //    return Result<List<T>>.Failure<E>(error);
    //}

    //if (info != null)
    //    return Result<List<T>>.Success(argument, info);

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
        )
    {
        return HasAnyValue(argument);
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

        Info info)
    {
        return HasAnyValue(argument, null, info);
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

        Error error)
    {
        return HasAnyValue(argumentrror);
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

        Error error,
        Info info)
    {
        return HasAnyValue(argumentrror, info);
    }

    /// <summary>
    /// Check IEnumerable's content and return the proper Result type.
    /// Depending on the Result and arguments: Log an Error and send an Exception or return the value and Log the Success Info
    /// </summary>
    /// <typeparam name="T">Generic value type of IEnumerable</typeparam>

    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<IEnumerable<T>> HasAny<T>(
        this IEnumerable<T> argument,
        )
    {
        return HasAnyValue<T>(argument);
    }

    /// <summary>
    /// Check IEnumerable's content and return the proper Result type.
    /// Depending on the Result: Log an Error and send an Exception or return the value and Log the Success Info
    /// </summary>
    /// <typeparam name="T">Generic value type of IEnumerable</typeparam>

    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="info">Fourth argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<IEnumerable<T>> HasAny<T>(
        this IEnumerable<T> argument,

        Info info)
    {
        return HasAnyValue<T>(argument, null, info);
    }

    /// <summary>
    /// Check IEnumerable's content and return the proper Result type.
    /// Depending on the Result: Log an Error and send an Exception or return the value and Log a Success Info
    /// </summary>
    /// <typeparam name="T">Generic value type of IEnumerable</typeparam>

    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="error">Third argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<IEnumerable<T>> HasAny<T>(
        this IEnumerable<T> argument,

        Error error)
    {
        return HasAnyValue<T>(argumentrror);
    }

    /// <summary>
    /// Check IEnumerable's content and return the proper Result type.
    /// Depending on the Result: Log an Error and send an Exception or return the value and Log the Success Info
    /// </summary>
    /// <typeparam name="T">Generic value type of IEnumerable</typeparam>

    /// <param name="argument">First argument</param>
    /// <param name="logger">Second argument</param>
    /// <param name="error">Third argument</param>
    /// <param name="info">Fourth argument</param>
    /// <returns>a new Failure Result with the Error or a new Success Result with the value</returns>
    public static Result<IEnumerable<T>> HasAny<T>(
        this IEnumerable<T> argument,

        Error error,
        Info info)
    {
        return HasAnyValue<T>(argumentrror, info);
    }

    private static Result<IEnumerable<T>> HasAnyValue<T>(
        IEnumerable<T> argument,

        Error? error = null,
        Info? info = null)
    {
        if (!argument.Any())
        {
            if (error == null)
                return Result<IEnumerable<T>>.Failure(Error.NullValue(""));

            return Result<IEnumerable<T>>.Failure(error);
        }

        if (info != null)
            return Result<IEnumerable<T>>.Success(argument, info);

        return Result<IEnumerable<T>>.Success(argument);
    }

    private static Result<IEnumerable<T>> HasAnyValue<T>(
        IEnumerable<T> argument,

        Error? error = null,
        Info? info = null)
    {
        if (!argument.Any())
        {
            if (error == null)
                return Result<IEnumerable<T>>.Failure<E>(Error.NullValue(""));

            return Result<IEnumerable<T>>.Failure<E>(error);
        }

        if (info != null)
            return Result<IEnumerable<T>>.Success(argument, info);

        return Result<IEnumerable<T>>.Success(argument);
    }
    #endregion
}

