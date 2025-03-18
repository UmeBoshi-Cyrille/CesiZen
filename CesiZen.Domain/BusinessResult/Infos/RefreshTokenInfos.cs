namespace CesiZen.Domain.BusinessResult;

public static class RefreshTokenInfos
{
    #region Log Messages
    public static Info LogInsertionSucceeded(string identifier) => new(InfoType.InsertionSucceeded, string.Format(Message.GetResource("InfoMessages", "LOG_INSERT_SUCCESS"), "RefreshToken", identifier));
    public static Info LogUpdateSucceeded(string id) => new(InfoType.UpdateSucceeded, string.Format(Message.GetResource("InfoMessages", "LOG_UPDATE_SUCCESS"), "RefreshToken", id));
    public static Info LogUpdateProperty(string property) => new(InfoType.UpdateSucceeded, string.Format(Message.GetResource("InfoMessages", "LOG_UPDATE_PROPERTY"), "RefreshToken", property));
    public static Info LogDeleteCompleted(string id) => new(InfoType.DeleteCompleted, string.Format(Message.GetResource("InfoMessages", "LOG_DELETION_SUCCESS"), "RefreshToken", id));
    public static Info LogCollectCompleted(string argument) => new(InfoType.CollectCompleted, string.Format(Message.GetResource("InfoMessages", "LOG_COLLECT_COMPLETED"), "RefreshToken", argument));
    public static Info LogFormatCompleted(string property) => new(InfoType.FormatCompleted, string.Format(Message.GetResource("InfoMessages", "LOG_FORMAT_COMPLETED"), "RefreshToken", property));
    #endregion
}
