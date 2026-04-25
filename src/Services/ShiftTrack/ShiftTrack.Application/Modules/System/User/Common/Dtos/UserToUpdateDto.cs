namespace ShiftTrack.Application.Modules.System.User.Common.Dtos;

public record UserToUpdateDto(
    string Id,
    string Email,
    string PhoneNumber);