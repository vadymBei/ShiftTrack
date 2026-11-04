using System.Text.RegularExpressions;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Services;

/// <summary>
/// Mentions are kept in the text as "&lt;@12&gt;" (the employee id), so a renamed employee shows the new name.
/// The client turns "@Олена Коваль" into the token when sending and the token into the name when showing.
/// </summary>
public static class MentionTokens
{
    private static readonly Regex TokenPattern = new(@"<@(\d{1,18})>", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    /// <summary>Mentioned ids in the order of the text, without repeats.</summary>
    public static IReadOnlyList<long> Parse(string text)
    {
        if (string.IsNullOrEmpty(text))
            return [];

        return TokenPattern.Matches(text)
            .Select(x => long.TryParse(x.Groups[1].Value, out var id) ? id : 0)
            .Where(x => x > 0)
            .Distinct()
            .ToList();
    }

    /// <summary>The text with "@Name" instead of the tokens, for notifications and previews.</summary>
    public static string ToPlainText(string text, Func<long, string> nameOf)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        return TokenPattern.Replace(text, match =>
            long.TryParse(match.Groups[1].Value, out var id) && nameOf(id) is { Length: > 0 } name
                ? "@" + name
                : "@працівник");
    }
}
