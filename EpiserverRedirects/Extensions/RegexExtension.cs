namespace Forte.EpiserverRedirects.Extensions;

public static class RegexExtension
{
    public static string ToStrictRegexPattern(this string originalPattern)
    {
        return $"^{originalPattern}$";
    }
}