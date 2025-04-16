namespace CesiZen.Domain.BusinessResult;

public static class UserErrors
{
    #region Client Messages
    public static Error ClientRegisterFailed => new(ErrorType.RegisterFailed, ResourceMessages.GetResource("ErrorMessages", "CLIENT_INSERTION_FAILED"));
    public static Error ClientUpdateFailed => new(ErrorType.UpdateFailed, ResourceMessages.GetResource("ErrorMessages", "CLIENT_UPDATE_FAILED"));
    public static Error ClientDeletionFailed => new(ErrorType.DeletionFailed, ResourceMessages.GetResource("ErrorMessages", "CLIENT_DELETION_FAILED"));
    public static Error ClientNotFound => new(ErrorType.NotFound, string.Format(ResourceMessages.GetResource("ErrorMessages", "CLIENT_NOTFOUND"), "Utilisateur"));
    public static Error ClientMultipleNotFound => new(ErrorType.NotFound, string.Format(ResourceMessages.GetResource("ErrorMessages", "CLIENT_MULTIPLE_NOTFOUND"), "Utilisateurs"));
    public static Error ClientAuthenticationFailed => new(ErrorType.AuthenticationFailed, ResourceMessages.GetResource("ErrorMessages", "CLIENT_AUTHENTICATION_FAILED"));
    public static Error ClientEmailVerificationFailed => new(ErrorType.EmailVerificationFailed, ResourceMessages.GetResource("ErrorMessages", "CLIENT_EMAIL_VERIFICATION_FAILED"));
    public static Error ClientExpiredLink => new(ErrorType.ExpiredLink, ResourceMessages.GetResource("ErrorMessages", "CLIENT_EXPIRED_LINK"));
    public static Error ClientLoginAttempsReached => new(ErrorType.LoginAttempsReached, ResourceMessages.GetResource("ErrorMessages", "CLIENT_LOGIN_ATTEMPS_REACHED"));
    public static Error ClientLockTime(string lockTime) => new(ErrorType.LockTime, string.Format(ResourceMessages.GetResource("ErrorMessages", "CLIENT_LOCKTIME"), lockTime));
    public static Error ClientResetPasswordAttempsReached => new(ErrorType.ResetPasswordAttempsReached, ResourceMessages.GetResource("ErrorMessages", "CLIENT_RESET_PASSWORD_ATTEMPS_REACHED"));
    public static Error ClientPasswordNotMatch => new(ErrorType.PasswordNotMatch, ResourceMessages.GetResource("ErrorMessages", "CLIENT_PASSWORD_NOTMATCH"));
    public static Error ClientNotUnique(string value) => new(ErrorType.NotUnique, string.Format(ResourceMessages.GetResource("ErrorMessages", "CLIENT_ACCOUNT_UNICITY_CONSTRAINT"), "L'email", value));
    public static Error ClientDisconnectFailed => new(ErrorType.DisconnectFailed, string.Format(ResourceMessages.GetResource("ErrorMessages", "CLIENT_DISCONNECTION_FAILED")));
    public static Error NotConnected => new(ErrorType.NotConnected, ResourceMessages.GetResource("ErrorMessages", "CLIENT_PLEASE_CONNECT"));
    public static Error Unknown => new(ErrorType.Unknown, ResourceMessages.GetResource("ErrorMessages", "CLIENT_UNKNOWN_USER"));
    #endregion

    #region Log Messages
    public static Error LogRegisterFailed(string identifier) => new(ErrorType.RegisterFailed, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_REGISTER_FAILED"), identifier));
    public static Error LogUpdateFailed(string id) => new(ErrorType.UpdateFailed, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_UPDATE_FAILED"), "User", id));
    public static Error LogUpdatePropertyFailed(string property, string id) => new(ErrorType.UpdatePropertyFailed, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_UPDATE_PROPERTY_FAILED"), $"User [Id:{id}]", property));
    public static Error LogDeletionFailed(string id) => new(ErrorType.DeletionFailed, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_DELETE_FAILED"), "User", id));
    public static Error LogNotFound(string id) => new(ErrorType.NotFound, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_GETONE_NOTFOUND"), "User", id));
    public static Error LogMultipleNotFound => new(ErrorType.NotFound, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_GET_MULTIPLE_NOTFOUND"), "Users"));
    public static Error LogLoginAttempsReached(string identifier) => new(ErrorType.LoginAttempsReached, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_LOGIN_ATTEMPS_REACHED"), identifier));
    public static Error LogResetPasswordAttempsReached(string identifier) => new(ErrorType.ResetPasswordAttempsReached, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_RESET_PASSWORD_ATTEMPS_REACHED"), identifier));
    public static Error LogEmailVerificationFailed(string email) => new(ErrorType.EmailVerificationFailed, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_EMAIL_VERIFICATION_FAILED"), email));
    public static Error LogLoginAttempsCount(string attemps, string email) => new(ErrorType.LoginAttempsCount, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_LOGIN_ATTEMPS_COUNT"), attemps, email));
    public static Error LogNotUnique(string value) => new(ErrorType.NotUnique, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_UNICITY_CONSTRAINT"), "User", value));
    #endregion
}
