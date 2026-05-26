namespace User.Authentication.Application.Modules.oAuth.Common.Dtos;

public record UserToUpdateDto(
    string Id,
    string Email,
    string PhoneNumber);