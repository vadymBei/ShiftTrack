namespace ShiftTrack.Application.Features.System.User.Common.Dtos;

public record UserToRegisterDto(
    string PhoneNumber,
    string Email,
    string Password);