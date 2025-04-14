namespace CesiZen.Domain.BusinessResult;

public class Info
{
    private readonly string message;
    private InfoType type;

    public string Message => !string.IsNullOrEmpty(message) ? message : string.Empty;
    public InfoType Type => type != InfoType.None ? type : type = InfoType.None;

    public Info()
    {
        message = string.Empty;
        type = InfoType.None;
    }

    public Info(InfoType type, string message)
    {
        this.message = message;
        this.type = type;
    }

    public static Info None => new(InfoType.None, string.Empty);
    public static Info Success(string message) => new(InfoType.Success, message);

}

public enum InfoType
{
    AccountEnabled,
    AccountDisabled,
    Authentified,
    CollectCompleted,
    DeleteCompleted,
    EmailVerified,
    EmailVerification,
    FormatCompleted,
    InsertionSucceeded,
    Logout,
    None,
    PasswordModified,
    ProcessComplete,
    ResetPasswordSucceed,
    ResetPasswordValid,
    Success,
    SessionClosed,
    UpdateProperty,
    UpdateSucceeded,
    VerificationEmailSent,
}

