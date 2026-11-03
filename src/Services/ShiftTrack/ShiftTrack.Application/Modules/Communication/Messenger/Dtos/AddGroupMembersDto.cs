namespace ShiftTrack.Application.Modules.Communication.Messenger.Dtos;

public record AddGroupMembersDto(
    IReadOnlyCollection<long> EmployeeIds);
