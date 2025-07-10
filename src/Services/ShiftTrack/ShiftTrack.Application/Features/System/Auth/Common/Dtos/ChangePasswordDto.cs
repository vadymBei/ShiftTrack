namespace ShiftTrack.Application.Features.System.Auth.Common.Dtos;

public record ChangePasswordDto(
    long EmployeeId,
    string OldPassword,
    string NewPassword,
    string ConfirmPassword);