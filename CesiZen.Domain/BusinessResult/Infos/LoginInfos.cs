namespace CesiZen.Domain.BusinessResult;

public class LoginInfos
{
    public static Info Authentified = new(InfoType.Authentified, Message.GetResource("InfoMessages", "CLIENT_AUTHENTICATION_SUCCESS"));
    public static Info EmailVerified = new(InfoType.EmailVerified, Message.GetResource("InfoMessages", "CLIENT_EMAIL_VERIFIED"));
    public static Info EmailVerification = new(InfoType.EmailVerification, Message.GetResource("InfoMessages", "CLIENT_EMAIL_VERIFICATION"));
    public static Info ResetPasswordSucceed = new(InfoType.ResetPasswordSucceed, Message.GetResource("InfoMessages", "CLIENT_RESETPASSWORD_SUCCESS"));
    public static Info ResetPasswordValid = new(InfoType.ResetPasswordValid, Message.GetResource("InfoMessages", "CLIENT_RESET_PASSWORD_VALID"));
    public static Info Logout = new(InfoType.Logout, Message.GetResource("InfoMessages", "CLIENT_LOGOUT"));
}
