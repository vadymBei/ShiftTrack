using ShiftTrack.Data.Interfaces;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Domain.Models.OAuth;

using EntityUser = ShiftTrack.Authentication.Models.User;

namespace User.Authentication.Core.Application.Common.Interfaces
{
    public interface IUserService : IEntityServiceBase<EntityUser>
    {
        Task<EntityUser> CreateUser(UserToCreateDto dto);

        Task<EntityUser> UpdateUser(UserToUpdateDto dto);

        Task<bool> CheckUserExist(string phoneNumber);

        Task<Token> ChangePassword(ChangePasswordDto dto, CancellationToken cancellationToken);
    }
}
