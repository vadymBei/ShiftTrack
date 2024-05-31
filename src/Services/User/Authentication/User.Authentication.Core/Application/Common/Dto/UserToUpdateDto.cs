namespace User.Authentication.Core.Application.Common.Dto
{
    public record UserToUpdateDto(
        string Id,
        string Email,
        string PhoneNumber);
}
