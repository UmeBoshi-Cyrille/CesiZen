namespace CesiZen.Domain.BusinessResult;

public static class LoginErrors
{
    public static Error AuthenticationFailed = new(ErrorType.AuthenticationFailed, Message.GetResource("ErrorMessages", "CLIENT_AUTHENTICATION_FAILED"));
    public static Error AuthenticationMismatch = new(ErrorType.AuthenticationMismatch, Message.GetResource("ErrorMessages", "CLIENT_AUTHENTICATION_MISMATCH"));
    public static Error EmailVerificationFailed = new(ErrorType.EmailVerificationFailed, Message.GetResource("ErrorMessages", "CLIENT_EMAIL_VERIFICATION_FAILED"));
    public static Error ExpiredLink = new(ErrorType.ExpiredLink, Message.GetResource("ErrorMessages", "CLIENT_EXPIRED_LINK"));
    public static Error ForgotPasswordAttempsReached = new(ErrorType.AttempsReached, Message.GetResource("ErrorMessages", "CLIENT_FORGOTPASSWORD_ATTEMPS"));
    public static Error LoginAttempsReached = new(ErrorType.AttempsReached, Message.GetResource("ErrorMessages", "CLIENT_LOGINATTEMPS"));
    public static Error LoginLockTime = new(ErrorType.LockTime, Message.GetResource("ErrorMessages", "CLIENT_LOGINATTEMPS_LOCKTIME"));
    public static Error PasswordNotMatch = new(ErrorType.NotMatch, Message.GetResource("ErrorMessages", "CLIENT_PASSWORD_NOTMATCH"));
    public static Error EmailNotUnique = new(ErrorType.NotUnique, Message.GetResource("ErrorMessages", "CLIENT_EMAIL_NOTUNIQUE"));
    public static Error UsernameNotUnique = new(ErrorType.LockTime, Message.GetResource("ErrorMessages", "CLIENT_USERNAME_NOTUNIQUE"));
    public static Error LoginNotFound = new(ErrorType.NotFound, Message.GetResource("ErrorMessages", "CLIENT_LOGIN_NOTFOUND"));
    public static Error ResetPasswordNotFound = new(ErrorType.NotFound, Message.GetResource("ErrorMessages", "CLIENT_RESETPASSWORD_NOTFOUND"));


    public static Error LogInsertionFailed(string title) => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_INSERTION_FAILED"), "Article", title));
    public static Error LogUpdateFailed(string id) => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_UPDATE_FAILED"), "Article", id));
    public static Error LogUpdatePropertyFailed(string property, string id) => new(ErrorType.UpdatePropertyFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_UPDATE_PROPERTY_FAILED"), $"Article [Id:{id}]", property));
    public static Error LogDeletionFailed(string id) => new(ErrorType.OperationFailed, string.Format(Message.GetResource("ErrorMessages", "LOG_DELETE_FAILED"), "Article", id));

    public static Error RegistrationFailed = new(ErrorType.RegistrationFailed, Message.GetResource("ErrorMessages", "LOG_REGISTER_OPERATIONFAILED"));



}
