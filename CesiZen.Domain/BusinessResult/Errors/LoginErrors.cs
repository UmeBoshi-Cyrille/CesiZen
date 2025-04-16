namespace CesiZen.Domain.BusinessResult;

public static class LoginErrors
{
    public static Error AuthenticationFailed => new(ErrorType.AuthenticationFailed, ResourceMessages.GetResource("ErrorMessages", "CLIENT_AUTHENTICATION_FAILED"));
    public static Error AuthenticationMismatch => new(ErrorType.AuthenticationMismatch, ResourceMessages.GetResource("ErrorMessages", "CLIENT_AUTHENTICATION_MISMATCH"));
    public static Error EmailVerificationFailed => new(ErrorType.EmailVerificationFailed, ResourceMessages.GetResource("ErrorMessages", "CLIENT_EMAIL_VERIFICATION_FAILED"));
    public static Error ExpiredLink => new(ErrorType.ExpiredLink, ResourceMessages.GetResource("ErrorMessages", "CLIENT_EXPIRED_LINK"));
    public static Error ForgotPasswordAttempsReached => new(ErrorType.AttempsReached, ResourceMessages.GetResource("ErrorMessages", "CLIENT_LOGINATTEMPS"));
    public static Error LoginLockTime => new(ErrorType.LockTime, ResourceMessages.GetResource("ErrorMessages", "CLIENT_LOGINATTEMPS_LOCKTIME"));
    public static Error PasswordNotMatch => new(ErrorType.NotMatch, ResourceMessages.GetResource("ErrorMessages", "CLIENT_PASSWORD_NOTMATCH"));
    public static Error EmailNotUnique => new(ErrorType.NotUnique, ResourceMessages.GetResource("ErrorMessages", "CLIENT_EMAIL_NOTUNIQUE"));
    public static Error UsernameNotUnique => new(ErrorType.LockTime, ResourceMessages.GetResource("ErrorMessages", "CLIENT_USERNAME_NOTUNIQUE"));
    public static Error LoginNotFound => new(ErrorType.NotFound, ResourceMessages.GetResource("ErrorMessages", "CLIENT_LOGIN_NOTFOUND"));
    public static Error ResetPasswordNotFound => new(ErrorType.NotFound, ResourceMessages.GetResource("ErrorMessages", "CLIENT_RESETPASSWORD_NOTFOUND"));


    public static Error LogInsertionFailed(string title) => new(ErrorType.OperationFailed, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_INSERTION_FAILED"), "Article", title));
    public static Error LogUpdateFailed(string id) => new(ErrorType.OperationFailed, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_UPDATE_FAILED"), "Article", id));
    public static Error LogUpdatePropertyFailed(string property, string id) => new(ErrorType.UpdatePropertyFailed, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_UPDATE_PROPERTY_FAILED"), $"Article [Id:{id}]", property));
    public static Error LogDeletionFailed(string id) => new(ErrorType.OperationFailed, string.Format(ResourceMessages.GetResource("ErrorMessages", "LOG_DELETE_FAILED"), "Article", id));

    public static Error RegistrationFailed => new(ErrorType.RegistrationFailed, ResourceMessages.GetResource("ErrorMessages", "LOG_REGISTER_OPERATIONFAILED"));
}
