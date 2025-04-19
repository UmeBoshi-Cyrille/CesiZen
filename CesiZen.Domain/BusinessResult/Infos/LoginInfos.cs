namespace CesiZen.Domain.BusinessResult;

public class LoginInfos
{
    public static Info ClientUpdateSucceeded => new(InfoType.UpdateSucceeded, string.Format(ResourceMessages.GetResource("InfoMessages", "CLIENT_UPDATE_SUCCESS"), "L''email'"));

    public static Info Authentified = new(InfoType.Authentified, ResourceMessages.GetResource("InfoMessages", "CLIENT_AUTHENTICATION_SUCCESS"));
    public static Info EmailVerified = new(InfoType.EmailVerified, ResourceMessages.GetResource("InfoMessages", "CLIENT_EMAIL_VERIFIED"));
    public static Info EmailVerification = new(InfoType.EmailVerification, ResourceMessages.GetResource("InfoMessages", "CLIENT_EMAIL_VERIFICATION"));
    public static Info ResetPasswordSucceed = new(InfoType.ResetPasswordSucceed, ResourceMessages.GetResource("InfoMessages", "CLIENT_RESETPASSWORD_SUCCESS"));
    public static Info ResetPasswordValid = new(InfoType.ResetPasswordValid, ResourceMessages.GetResource("InfoMessages", "CLIENT_RESET_PASSWORD_VALID"));
    public static Info Logout = new(InfoType.Logout, ResourceMessages.GetResource("InfoMessages", "CLIENT_LOGOUT"));
    public static Info CookieDeleted = new(InfoType.CookieDeleted, ResourceMessages.GetResource("InfoMessages", "CLIENT_DELETE_COOKIE"));
}
