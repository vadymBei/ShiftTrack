using User.Authentication.Application.Features.oAuth.Common.Dtos;

namespace User.Authentication.Application.Features.oAuth.Common.Interfaces;

public interface IUserRoleService
{
    Task CreateUserRole(UserRoleToCreateDto dto);
}