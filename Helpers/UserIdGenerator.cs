namespace SmartEMS.API.Helpers;

public static class UserIdGenerator
{
    public static string GenerateUserId(string prefix, int number)
    {
        return $"{prefix}{number.ToString("D3")}";
    }
}