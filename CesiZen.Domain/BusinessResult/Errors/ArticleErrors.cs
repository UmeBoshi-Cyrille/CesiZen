namespace CesiZen.Domain.BusinessResult;
public static class ArticleErrors
{
    #region Client Messages
    public static Error ClientInsertionFailed => new(ErrorType.OperationFailed, string.Format(ResourceMessages.GetResource("ErrorMessages", "CLIENT_INSERTION_FAILED"), "Article"));
    public static Error ClientUpdateFailed => new(ErrorType.OperationFailed, string.Format(ResourceMessages.GetResource("ErrorMessages", "CLIENT_UPDATE_FAILED"), "Article"));
    public static Error ClientDeletionFailed => new(ErrorType.OperationFailed, string.Format(ResourceMessages.GetResource("ErrorMessages", "CLIENT_DELETION_FAILED"), "Article"));
    public static Error ClientNotFound => new(ErrorType.NotFound, string.Format(ResourceMessages.GetResource("ErrorMessages", "CLIENT_NOTFOUND"), "Article"));
    public static Error ClientMultipleNotFound => new(ErrorType.NotFound, string.Format(ResourceMessages.GetResource("ErrorMessages", "CLIENT_NOTFOUND"), "Articles"));
    #endregion

    #region Log Messages
    public static Error LogInsertionFailed(string title) => new(ErrorType.OperationFailed, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_INSERTION_FAILED"), "Article", title));
    public static Error LogUpdateFailed(string id) => new(ErrorType.OperationFailed, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_UPDATE_FAILED"), "Article", id));
    public static Error LogUpdatePropertyFailed(string property, string id) => new(ErrorType.UpdatePropertyFailed, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_UPDATE_PROPERTY_FAILED"), $"Article [Id:{id}]", property));
    public static Error LogDeletionFailed(string id) => new(ErrorType.OperationFailed, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_DELETE_FAILED"), "Article", id));
    public static Error LogNotFound(string id) => new(ErrorType.NotFound, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "Article", id));
    public static Error LogMultipleNotFound => new(ErrorType.NotFound, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_GET_MULTIPLE_NOTFOUND"), "Articles"));
    #endregion
}