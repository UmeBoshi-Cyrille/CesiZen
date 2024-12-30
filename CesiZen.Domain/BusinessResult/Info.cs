namespace CesiZen.Domain.BusinessResult;

public class Info
{
    private readonly string message;
    private readonly InfoType type;

    public string Message => message;
    public InfoType Type => type;

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
    None,
    Authentified,
    ProcessComplete,
    StepComplete,
    Success,
}

