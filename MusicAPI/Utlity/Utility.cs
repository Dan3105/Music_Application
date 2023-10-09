namespace MusicAPI.Utlity;
public class Utility
{
    public static bool IsInDebugMode()
    {
        var environmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        return environmentVariable == "Development";
    }
}
