namespace CesiZen.Domain.BusinessResult;
public static class ArticleErrors
{
    #region Client Messages
    public static Error InsertionFailed => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "CLIENT_INSERTION_FAILED"), "Article"));
    public static Error UpdateFailed => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "CLIENT_UPDATE_FAILED"), "Article"));
    public static Error DeletionFailed => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "CLIENT_DELETION_FAILED"), "Article"));
    public static Error NotFound => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "CLIENT_NOTFOUND"), "Article"));
    public static Error MultipleNotFound => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "CLIENT_NOTFOUND"), "Articles"));
    #endregion

    #region Log Messages
    public static Error InsertionFailedToLog => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_INSERTION_OPERATIONFAILED"), "Article"));
    public static Error UpdateFailedToLog => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_UPDATE_OPERATIONFAILED"), "Article"));
    public static Error DeletionFailedToLog => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_DELETE_OPERATIONFAILED"), "Article"));
    public static Error NotFoundToLog => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "Article"));
    public static Error MultipleNotFoundToLog => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "LOG_GET_MULTIPLE_NOTFOUND"), "Articles"));

    public static Error OperationFailed(string action, string message) => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_OPERATIONFAILED"), action, message));
    #endregion
}
