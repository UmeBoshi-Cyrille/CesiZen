namespace CesiZen.Application.Services;

public static class TimeService
{
    public static int CalculateLockTime(DateTime? lockoutEndTime)
    {
        TimeSpan remainingTime = (TimeSpan)(lockoutEndTime - DateTime.UtcNow)!;

        return (int)remainingTime.TotalMinutes;
    }
}
