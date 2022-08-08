namespace task1.Helpers;

public static class AccountHelper
{
    public static long Parse(string v)
    {
        return long.Parse(v.TrimStart('“').TrimEnd('”'));
    }
}
