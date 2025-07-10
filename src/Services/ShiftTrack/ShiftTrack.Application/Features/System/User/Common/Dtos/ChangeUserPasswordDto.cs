namespace ShiftTrack.Application.Features.System.User.Common.Dtos;

public record ChangeUserPasswordDto(
    string UserId,
    string OldPassword,
    string NewPassword,
    string ConfirmPassword);