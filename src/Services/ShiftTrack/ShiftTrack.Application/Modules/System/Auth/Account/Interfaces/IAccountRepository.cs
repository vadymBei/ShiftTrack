using ShiftTrack.Application.Modules.System.Auth.Account.Dtos;
using ShiftTrack.Domain.Modules.System.Auth.Models;

namespace ShiftTrack.Application.Modules.System.Auth.Account.Interfaces;

public interface IAccountRepository
{
    Task<Authentication.Models.User> RegisterUser(UserToRegisterDto dto, CancellationToken cancellationToken);
    Task<Authentication.Models.User> UpdateUser(UserToUpdateDto dto, CancellationToken cancellationToken);
    Task<Token> ChangePassword(ChangeUserPasswordDto dto, CancellationToken cancellationToken);
}