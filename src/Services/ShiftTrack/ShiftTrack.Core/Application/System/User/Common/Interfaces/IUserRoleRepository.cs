using ShiftTrack.Core.Application.System.User.Common.Dtos;

namespace ShiftTrack.Core.Application.System.User.Common.Interfaces
{
    public interface IUserRoleRepository
    {
        Task CreateUserRole(UserRoleToCreateDto dto, CancellationToken cancellationToken);
    }
}
