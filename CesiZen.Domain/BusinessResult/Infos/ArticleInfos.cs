namespace CesiZen.Domain.BusinessResult;

public static class ArticleInfos
{
    #region Client Messages
    public static Info InsertionSucceeded => new(InfoType.InsertionSucceeded, string.Format(Message.GetResource("InfoMessages", "CLIENT_INSERTION_SUCCESS"), "Votre Article"));
    public static Info UpdateSucceeded => new(InfoType.UpdateSucceeded, string.Format(Message.GetResource("InfoMessages", "CLIENT_UPDATE_SUCCESS"), "L'article"));
    public static Info DeleteCompleted => new(InfoType.DeleteCompleted, string.Format(Message.GetResource("InfoMessages", "CLIENT_DELETE_SUCCESS"), "L'article"));
    #endregion

    #region Log Messages
    public static Info LogInsertionSucceeded(string identifier) => new(InfoType.InsertionSucceeded, string.Format(Message.GetResource("InfoMessages", "LOG_INSERT_SUCCESS"), "Article", identifier));
    public static Info LogUpdateSucceeded => new(InfoType.UpdateSucceeded, string.Format(Message.GetResource("InfoMessages", "LOG_UPDATE_SUCCESS"), "Article"));
    public static Info LogUpdateProperty(string property) => new(InfoType.UpdateSucceeded, string.Format(Message.GetResource("InfoMessages", "LOG_UPDATE_PROPERTY"), "Article", property));
    public static Info LogDeleteCompleted => new(InfoType.DeleteCompleted, string.Format(Message.GetResource("InfoMessages", "LOG_DELETION_SUCCESS"), "Article"));
    public static Info LogCollectCompleted(string argument) => new(InfoType.CollectCompleted, string.Format(Message.GetResource("InfoMessages", "LOG_COLLECT_COMPLETED"), "Article", argument));
    public static Info LogFormatCompleted(string property) => new(InfoType.FormatCompleted, string.Format(Message.GetResource("InfoMessages", "LOG_FORMAT_COMPLETED"), "Article", property));
    #endregion
}
