namespace User.Authentication.Application.Features.oAuth.Common.Dtos;

public record UserRoleToCreateDto(
    string UserId,
    string RoleId);