namespace CesiZen.Domain.BusinessResult;

public sealed class Error
{
    private string message;
    private ErrorType type;

    public string Message => type != ErrorType.None && !string.IsNullOrEmpty(message) ? message : string.Empty;
    public ErrorType Type => type != ErrorType.None ? type : type = ErrorType.None;

    public Error(ErrorType type, string message)
    {
        this.message = message;
        this.type = type;
    }

    public static Error None => new(ErrorType.None, string.Empty);
    public static Error NotFound(string message) => new(ErrorType.NotFound, message);
    public static Error NullValue(string message) => new(ErrorType.NullValue, message);
    public static Error AuthenticationFailed(string message) => new(ErrorType.AuthenticationFailed, message);
    public static Error OperationFailed(string message) => new(ErrorType.OperationFailed, message);
    public static Error NotUnique(string message) => new(ErrorType.NotUnique, message);
    public static Error WrongFormat(string message) => new(ErrorType.WrongFormat, message);
    public static Error NotMatch(string message) => new(ErrorType.NotMatch, message);
    public static Error NullOrWhiteSpace(string message) => new(ErrorType.NullOrWhiteSpace, message);
    public static Error NullOrEmpty(string message) => new(ErrorType.NullOrEmpty, message);
    public static Error TimeOut(string message) => new(ErrorType.TimeOut, message);

}

public enum ErrorType
{
    None,
    NotFound,
    NullValue,
    AuthenticationFailed,
    OperationFailed,
    NotUnique,
    WrongFormat,
    NotMatch,
    NullOrWhiteSpace,
    NullOrEmpty,
    TimeOut
}