namespace ShiftTrack.Application.Modules.System.Auth.Common.Dtos;

public record ChangePasswordDto(
    long EmployeeId,
    string OldPassword,
    string NewPassword,
    string ConfirmPassword);