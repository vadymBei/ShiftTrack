namespace User.Authentication.Application.Features.oAuth.Common.Dtos;

public record UserToUpdateDto(
    string Id,
    string Email,
    string PhoneNumber);