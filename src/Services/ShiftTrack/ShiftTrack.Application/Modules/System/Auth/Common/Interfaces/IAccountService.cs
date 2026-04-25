using ShiftTrack.Application.Modules.System.Auth.Common.Dtos;
using ShiftTrack.Application.Modules.System.User.Common.Dtos;
using ShiftTrack.Domain.Modules.System.Auth.Models;

namespace ShiftTrack.Application.Modules.System.Auth.Common.Interfaces;

public interface IAccountService
{
    Task<Authentication.Models.User> RegisterAuthUser(UserToRegisterDto dto, CancellationToken cancellationToken);
    Task<Authentication.Models.User> UpdateAuthUser(UserToUpdateDto dto, CancellationToken cancellationToken);
    Task<Token> ChangePassword(ChangePasswordDto dto, CancellationToken cancellationToken);

}