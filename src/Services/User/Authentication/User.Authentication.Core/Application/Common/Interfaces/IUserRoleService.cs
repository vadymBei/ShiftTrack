using User.Authentication.Core.Application.Common.Dto;

namespace User.Authentication.Core.Application.Common.Interfaces
{
    public interface IUserRoleService
    {
        Task CreateUserRole(UserRoleToCreateDto dto);
    }
}
