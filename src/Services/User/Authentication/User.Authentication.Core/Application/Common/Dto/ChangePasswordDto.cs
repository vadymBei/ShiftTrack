namespace User.Authentication.Core.Application.Common.Dto
{
    public record ChangePasswordDto(
        string UserId,
        string OldPassword,
        string NewPassword,
        string ConfirmPassword);
}
