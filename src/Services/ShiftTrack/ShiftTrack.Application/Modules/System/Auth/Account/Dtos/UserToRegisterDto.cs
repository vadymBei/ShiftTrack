namespace ShiftTrack.Application.Modules.System.Auth.Account.Dtos;

public record UserToRegisterDto(
    string PhoneNumber,
    string Email,
    string Password);