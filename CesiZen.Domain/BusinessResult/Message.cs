using System.Reflection;
using System.Resources;

namespace CesiZen.Domain.BusinessResult;

public static class Message
{
    public static string GetResource(string fileName, string resourceKey)
    {
        ResourceManager rm = new ResourceManager($"RE.Domain.Resources.{fileName.Replace(".resx", "")}", Assembly.GetExecutingAssembly());
        return rm.GetString(resourceKey)!;
    }
}
