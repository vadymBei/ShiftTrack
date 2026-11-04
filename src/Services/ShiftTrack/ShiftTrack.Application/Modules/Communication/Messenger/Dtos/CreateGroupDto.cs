namespace ShiftTrack.Application.Modules.Communication.Messenger.Dtos;

public record CreateGroupDto(
    string Title,
    IReadOnlyCollection<long> MemberIds);
