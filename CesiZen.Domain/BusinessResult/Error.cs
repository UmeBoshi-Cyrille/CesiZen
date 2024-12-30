namespace CesiZen.Domain.BusinessResult;

public sealed class Error
{
    private string message;
    private ErrorType type;

    public string Message => type != ErrorType.None && !string.IsNullOrEmpty(message) ? message : string.Empty;
    public ErrorType Type => type != ErrorType.None ? type : ErrorType.None;

    public Error(ErrorType type, string message)
    {
        this.message = message;
        this.type = type;
    }

    public static Error None => new(ErrorType.None, string.Empty);

    public static Error NotFound(string message) => new(ErrorType.NotFound, message);

    public static Error NullValue(string message) => new(ErrorType.NullValue, message);

    public static Error InvalidArgument<TValue>(TValue? value, string? message) => new(ErrorType.InvalidArgument, $"{value}: {message}");
    public static Error NullOrEmpty<TValue>(TValue? value, string? message) => new(ErrorType.NullOrEmpty, $"{value}: {message}");
    public static Error NullOrWhiteSpace<TValue>(TValue? value, string? message) => new(ErrorType.NullOrWhiteSpace, $"{value}: {message}");
    public static Error EmptyValue<TValue>(TValue? value, string? message) => new(ErrorType.EmptyValue, $"{value}: {message}");

}

public enum ErrorType
{
    None,
    NotFound,
    InvalidArgument,
    NullValue,
    NullOrEmpty,
    EmptyValue,
    NullOrWhiteSpace,
}