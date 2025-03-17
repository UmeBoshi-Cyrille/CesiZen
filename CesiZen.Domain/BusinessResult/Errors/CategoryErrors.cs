namespace CesiZen.Domain.BusinessResult;

public static class CategoryErrors
{
    #region Client Messages
    public static Error InsertionFailed => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "CLIENT_INSERTION_FAILED"), "Categorie"));
    public static Error UpdateFailed => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "CLIENT_UPDATE_FAILED"), "Categorie"));
    public static Error DeletionFailed => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "CLIENT_DELETION_FAILED"), "Categorie"));
    public static Error NotFound => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "CLIENT_NOTFOUND"), "Categorie"));
    public static Error MultipleNotFound => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "CLIENT_NOTFOUND"), "Categories"));
    #endregion

    #region Log Messages
    public static Error InsertionFailedToLog => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_INSERTION_OPERATIONFAILED"), "Category"));
    public static Error UpdateFailedToLog => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_UPDATE_OPERATIONFAILED"), "Category"));
    public static Error DeletionFailedToLog => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_DELETE_OPERATIONFAILED"), "Category"));
    public static Error NotFoundToLog => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "Category"));
    public static Error MultipleNotFoundToLog => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "LOG_GET_MULTIPLE_NOTFOUND"), "Categorys"));
    #endregion
}
