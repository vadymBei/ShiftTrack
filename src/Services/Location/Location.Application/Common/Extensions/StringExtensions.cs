namespace Location.Application.Common.Extensions;

public static class StringExtensions
{
    public static string NormalizeString(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        return input
            .Trim()
            .ToLowerInvariant()
            .Replace("'", "")
            .Replace("’", "")
            .Replace("-", " ");
    }
}