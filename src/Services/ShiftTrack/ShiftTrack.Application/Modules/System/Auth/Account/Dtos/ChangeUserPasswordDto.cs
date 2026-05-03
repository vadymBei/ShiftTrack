namespace ShiftTrack.Application.Modules.System.Auth.Account.Dtos;

public record ChangeUserPasswordDto(
    string UserId,
    string OldPassword,
    string NewPassword,
    string ConfirmPassword);