namespace User.Authentication.Application.Modules.oAuth.Common.Dtos;

public record ChangePasswordDto(
    string UserId,
    string OldPassword,
    string NewPassword,
    string ConfirmPassword);