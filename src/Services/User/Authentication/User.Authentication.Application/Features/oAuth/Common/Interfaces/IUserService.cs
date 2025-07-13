using ShiftTrack.Data.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.Dtos;
using User.Authentication.Domain.Features.oAuth.Models;

using EntityUser = ShiftTrack.Authentication.Models.User;

namespace User.Authentication.Application.Features.oAuth.Common.Interfaces;

public interface IUserService : IEntityServiceBase<EntityUser>
{
    Task<EntityUser> CreateUser(UserToCreateDto dto);
    Task<EntityUser> UpdateUser(UserToUpdateDto dto);
    Task<bool> CheckUserExist(string phoneNumber);
    Task<Token> ChangePassword(ChangePasswordDto dto, CancellationToken cancellationToken);
    Task<IEnumerable<EntityUser>> GetUsers(CancellationToken cancellationToken);
}