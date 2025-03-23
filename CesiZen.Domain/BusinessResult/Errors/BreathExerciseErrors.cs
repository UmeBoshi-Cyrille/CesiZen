namespace CesiZen.Domain.BusinessResult;

public static class BreathExerciseErrors
{
    #region Client Messages
    public static Error ClientInsertionFailed => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "CLIENT_INSERTION_FAILED"), "Exercise"));
    public static Error ClientUpdateFailed => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "CLIENT_UPDATE_FAILED"), "Exercise"));
    public static Error ClientDeletionFailed => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "CLIENT_DELETION_FAILED"), "Exercise"));
    public static Error ClientNotFound => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "CLIENT_NOTFOUND"), "Exercise"));
    public static Error ClientMultipleNotFound => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "CLIENT_NOTFOUND"), "Exercises"));
    #endregion

    #region Log Messages
    public static Error LogInsertionFailed(string title) => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_INSERTION_FAILED"), "Exercise", title));
    public static Error LogUpdateFailed(int Id) => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_UPDATE_FAILED"), "Exercise", id));
    public static Error LogUpdatePropertyFailed(string property, int Id) => new(ErrorType.UpdatePropertyFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_UPDATE_PROPERTY_FAILED"), $"Exercise [Id:{id}]", property));
    public static Error LogDeletionFailed(int Id) => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_DELETE_FAILED"), "Exercise", id));
    public static Error LogNotFound(int Id) => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "Exercise", id));
    public static Error LogMultipleNotFound => new(ErrorType.NotFound, string.Format(Message.GetResource("ErrorMessages", "LOG_GET_MULTIPLE_NOTFOUND"), "Exercises"));
    #endregion
}
