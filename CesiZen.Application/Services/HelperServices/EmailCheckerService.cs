using System.Text.RegularExpressions;

namespace CesiZen.Application.Services;

public static class EmailCheckerService
{
    public static bool IsValidEmail(this string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]{2,3}$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }
}
