namespace User.Authentication.Application.Modules.oAuth.Common.Dtos;

public record UserToCreateDto(
    string Email,
    string PhoneNumber,
    string Password);