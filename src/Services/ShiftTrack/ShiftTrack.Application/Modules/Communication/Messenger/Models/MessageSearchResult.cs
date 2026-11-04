namespace ShiftTrack.Application.Modules.Communication.Messenger.Models;

/// <summary>Ids of the matching messages, the newest first.</summary>
public record MessageSearchResult(
    IReadOnlyList<long> MessageIds,
    bool HasMore);
