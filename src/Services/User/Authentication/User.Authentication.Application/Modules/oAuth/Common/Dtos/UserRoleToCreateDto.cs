namespace User.Authentication.Application.Modules.oAuth.Common.Dtos;

public record UserRoleToCreateDto(
    string UserId,
    string RoleId);