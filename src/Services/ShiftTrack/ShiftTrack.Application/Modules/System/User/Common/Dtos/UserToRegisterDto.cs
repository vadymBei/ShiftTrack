namespace ShiftTrack.Application.Modules.System.User.Common.Dtos;

public record UserToRegisterDto(
    string PhoneNumber,
    string Email,
    string Password);