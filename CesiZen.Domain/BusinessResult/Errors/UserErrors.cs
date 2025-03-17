namespace CesiZen.Domain.BusinessResult;

public static class UserErrors
{
    #region Client Messages
    public static Error InsertionFailed => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "CLIENT_INSERTION_FAILED"), "User"));
    public static Error UpdateFailed => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "CLIENT_UPDATE_FAILED"), "User"));
    public static Error DeletionFailed => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "CLIENT_DELETION_FAILED"), "User"));
    public static Error NotFound => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "CLIENT_NOTFOUND"), "User"));
    public static Error MultipleNotFound => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "CLIENT_NOTFOUND"), "Users"));
    #endregion

    #region Log Messages
    public static Error InsertionFailedToLog => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_INSERTION_OPERATIONFAILED"), "User"));
    public static Error UpdateFailedToLog => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_UPDATE_OPERATIONFAILED"), "User"));
    public static Error DeletionFailedToLog => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_DELETE_OPERATIONFAILED"), "User"));
    public static Error NotFoundToLog => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "User"));
    public static Error MultipleNotFoundToLog => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "LOG_GET_MULTIPLE_NOTFOUND"), "Users"));
    #endregion
}
