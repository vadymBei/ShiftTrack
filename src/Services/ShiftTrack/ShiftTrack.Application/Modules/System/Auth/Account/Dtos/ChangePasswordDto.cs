namespace ShiftTrack.Application.Modules.System.Auth.Account.Dtos;

public record ChangePasswordDto(
    long EmployeeId,
    string OldPassword,
    string NewPassword,
    string ConfirmPassword);