using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Domain.System.Tokens.Models;

namespace ShiftTrack.Core.Application.System.User.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<Authentication.Models.User> RegisterUser(UserToRegisterDto dto, CancellationToken cancellationToken);

        Task<Authentication.Models.User> UpdateUser(UserToUpdateDto dto, CancellationToken cancellationToken);

        Task<Token> ChangePassword(ChangeUserPasswordDto dto, CancellationToken cancellationToken);
    }
}
