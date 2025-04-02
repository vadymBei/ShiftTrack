namespace ShiftTrack.Core.Application.System.User.Common.Dtos;

public record ChangeEmployeePasswordDto(
    long EmployeeId,
    string OldPassword,
    string NewPassword,
    string ConfirmPassword);