namespace CesiZen.Domain.BusinessResult;

public static class CategoryInfos
{
    #region Client Messages
    public static Info ClientInsertionSucceeded => new(InfoType.InsertionSucceeded, string.Format(Message.GetResource("InfoMessages", "CLIENT_INSERTION_SUCCESS"), "La catégorie"));
    public static Info ClientUpdateSucceeded => new(InfoType.UpdateSucceeded, string.Format(Message.GetResource("InfoMessages", "CLIENT_UPDATE_SUCCESS"), "La catégorie"));
    public static Info ClientDeleteCompleted => new(InfoType.DeleteCompleted, string.Format(Message.GetResource("InfoMessages", "CLIENT_DELETE_SUCCESS"), "La catégorie"));
    #endregion

    #region Log Messages
    public static Info LogInsertionSucceeded(int Identifier) => new(InfoType.InsertionSucceeded, string.Format(Message.GetResource("InfoMessages", "LOG_INSERT_SUCCESS"), "Category", identifier));
    public static Info LogUpdateSucceeded(int Id) => new(InfoType.UpdateSucceeded, string.Format(Message.GetResource("InfoMessages", "LOG_UPDATE_SUCCESS"), "Category", id));
    public static Info LogUpdateProperty(string property) => new(InfoType.UpdateSucceeded, string.Format(Message.GetResource("InfoMessages", "LOG_UPDATE_PROPERTY"), "Category", property));
    public static Info LogDeleteCompleted(int Id) => new(InfoType.DeleteCompleted, string.Format(Message.GetResource("InfoMessages", "LOG_DELETION_SUCCESS"), "Category", id));
    public static Info LogCollectCompleted(string argument) => new(InfoType.CollectCompleted, string.Format(Message.GetResource("InfoMessages", "LOG_COLLECT_COMPLETED"), "Category", argument));
    public static Info LogFormatCompleted(string property) => new(InfoType.FormatCompleted, string.Format(Message.GetResource("InfoMessages", "LOG_FORMAT_COMPLETED"), "Category", property));
    #endregion
}
