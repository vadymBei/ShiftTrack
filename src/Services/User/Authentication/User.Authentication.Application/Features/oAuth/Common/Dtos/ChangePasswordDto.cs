namespace User.Authentication.Application.Features.oAuth.Common.Dtos;

public record ChangePasswordDto(
    string UserId,
    string OldPassword,
    string NewPassword,
    string ConfirmPassword);