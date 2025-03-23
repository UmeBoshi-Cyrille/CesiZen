namespace CesiZen.Domain.BusinessResult;

public static class RefreshTokenErrors
{
    #region Log Messages
    public static Error LogInsertionFailed(string title) => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_INSERTION_FAILED"), "RefreshToken", title));
    public static Error LogUpdateFailed(string id) => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_UPDATE_FAILED"), "RefreshToken", id));
    public static Error LogUpdatePropertyFailed(string property, string id) => new(ErrorType.UpdatePropertyFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_UPDATE_PROPERTY_FAILED"), $"RefreshToken [Id:{id}]", property));
    public static Error LogDeletionFailed(string id) => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_DELETE_FAILED"), "RefreshToken", id));
    public static Error LogNotFound(string id) => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "RefreshToken", id));
    public static Error LogMultipleNotFound => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "LOG_GET_MULTIPLE_NOTFOUND"), "RefreshTokens"));
    public static Error LogNotUnique(string value) => new(ErrorType.NotUnique, string.Format(Message.GetResource("ErrorMessages", "LOG_UNICITY_CONSTRAINT"), "RefreshToken", value));
    #endregion
}
