namespace ShiftTrack.Application.Modules.System.Auth.Account.Dtos;

public record UserToUpdateDto(
    string Id,
    string Email,
    string PhoneNumber);