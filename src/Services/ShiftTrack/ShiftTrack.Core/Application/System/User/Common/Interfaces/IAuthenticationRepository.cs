using ShiftTrack.Core.Application.System.User.Common.Dtos;

namespace ShiftTrack.Core.Application.System.User.Common.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<Authentication.Models.User> RegisterUser(UserToRegisterDto dto, CancellationToken cancellationToken);
    }
}
