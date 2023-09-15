namespace ScrumApp.Helpers;

public static class HtmlHelpers
{
    static readonly Random random = new();
    public static string GetRandomHtmlId(int length = 10)
    {
        // valid characters
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());
    }
    public static string RandomCacheKey
        => (random.Next(int.MaxValue - 1111111111) + 1111111111).ToString();
}
