namespace User.Authentication.Application.Features.oAuth.Common.Dtos;

public record UserToCreateDto(
    string Email,
    string PhoneNumber,
    string Password);