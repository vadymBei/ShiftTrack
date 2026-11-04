namespace ShiftTrack.Application.Modules.Communication.Messenger.Models;

/// <summary>A message that may match a search: what is shown of it, without the rest.</summary>
public record MessageSearchCandidate(
    long Id,
    string Text,
    IReadOnlyList<string> FileNames,
    IReadOnlyDictionary<long, string> MentionLabels);
