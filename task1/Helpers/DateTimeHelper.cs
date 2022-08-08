namespace task1.Helpers;

public static class DateTimeHelper
{
    public static string Reverse(string s)
    {
        var tokens = s.Split("-").Reverse();
        return string.Join("/", tokens);
    }
}
