using User.Authentication.Application.Modules.oAuth.Common.Dtos;

namespace User.Authentication.Application.Modules.oAuth.Common.Interfaces;

public interface IUserRoleService
{
    Task CreateUserRole(UserRoleToCreateDto dto);
}