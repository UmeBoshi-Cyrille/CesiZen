namespace CesiZen.Domain.BusinessResult;

public static class UserInfos
{
    #region Client Messages
    public static Info ClientInsertionSucceeded => new(InfoType.InsertionSucceeded, string.Format(Message.GetResource("InfoMessages", "CLIENT_INSERTION_SUCCESS"), "Votre Compte"));
    public static Info ClientUpdateSucceeded => new(InfoType.UpdateSucceeded, string.Format(Message.GetResource("InfoMessages", "CLIENT_UPDATE_SUCCESS"), "L'utilisateur"));
    public static Info ClientDeleteCompleted => new(InfoType.DeleteCompleted, string.Format(Message.GetResource("InfoMessages", "CLIENT_DELETE_SUCCESS"), "L'utilisateur"));
    public static Info ClientSessionClosed => new(InfoType.SessionClosed, Message.GetResource("InfoMessages", "CLIENT_SESSION_CLOSED"));
    public static Info ClientEmailVerified => new(InfoType.EmailVerified, Message.GetResource("InfoMessages", "CLIENT_EMAIL_VERIFIED"));
    public static Info ClientVerificationEmailSent => new(InfoType.VerificationEmailSent, Message.GetResource("InfoMessages", "CLIENT_EMAIL_VERIFICATION"));
    public static Info ClientAuthentified => new(InfoType.Authentified, Message.GetResource("InfoMessages", "CLIENT_AUTHENTIFIED"));
    public static Info ClientPasswordModified => new(InfoType.PasswordModified, Message.GetResource("InfoMessages", "LOG_PASSWORD_MODIFICATION"));
    #endregion

    #region Log Messages
    public static Info LogInsertionSucceeded(string identifier) => new(InfoType.InsertionSucceeded, string.Format(Message.GetResource("InfoMessages", "LOG_INSERT_SUCCESS"), "User", identifier));
    public static Info LogUpdateSucceeded(string id) => new(InfoType.UpdateSucceeded, string.Format(Message.GetResource("InfoMessages", "LOG_UPDATE_SUCCESS"), "User", id));
    public static Info LogUpdateProperty(string property, string id) => new(InfoType.UpdateSucceeded, string.Format(Message.GetResource("InfoMessages", "LOG_UPDATE_PROPERTY"), "User", property, id));
    public static Info LogDeleteCompleted(string id) => new(InfoType.DeleteCompleted, string.Format(Message.GetResource("InfoMessages", "LOG_DELETION_SUCCESS"), "User", id));
    public static Info LogCollectCompleted(string argument) => new(InfoType.CollectCompleted, string.Format(Message.GetResource("InfoMessages", "LOG_COLLECT_COMPLETED"), "User", argument));
    public static Info LogFormatCompleted(string property) => new(InfoType.FormatCompleted, string.Format(Message.GetResource("InfoMessages", "LOG_FORMAT_COMPLETED"), "User", property));
    public static Info LogSessionClosed(string identifier) => new(InfoType.SessionClosed, string.Format(Message.GetResource("InfoMessages", "LOG_SESSION_CLOSED"), identifier));
    public static Info LogEmailVerified(string email) => new(InfoType.EmailVerified, string.Format(Message.GetResource("InfoMessages", "LOG_EMAIL_VERIFIED"), email));
    public static Info LogVerificationEmailSent(string email) => new(InfoType.EmailVerified, string.Format(Message.GetResource("InfoMessages", "LOG_EMAIL_VERIFICATION"), email));
    public static Info LogAuthentified(string identifier) => new(InfoType.Authentified, string.Format(Message.GetResource("InfoMessages", "LOG_AUTHENTIFIED"), identifier));
    public static Info LogPasswordModified(string identifier) => new(InfoType.PasswordModified, string.Format(Message.GetResource("InfoMessages", "LOG_PASSWORD_MODIFICATION"), identifier));
    #endregion
}
