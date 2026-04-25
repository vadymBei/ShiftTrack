using ShiftTrack.Application.Modules.System.User.Common.Dtos;
using ShiftTrack.Domain.Modules.System.Auth.Models;

namespace ShiftTrack.Application.Modules.System.User.Common.Interfaces;

public interface IUserRepository
{
    Task<Authentication.Models.User> RegisterUser(UserToRegisterDto dto, CancellationToken cancellationToken);
    Task<Authentication.Models.User> UpdateUser(UserToUpdateDto dto, CancellationToken cancellationToken);
    Task<Token> ChangePassword(ChangeUserPasswordDto dto, CancellationToken cancellationToken);
}