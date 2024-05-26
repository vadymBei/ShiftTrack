using User.Authentication.Core.Application.Common.Dto;

namespace User.Authentication.Core.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<ShiftTrack.Authentication.Models.User> CreateUser(UserToCreateDto dto);

        Task<bool> CheckUserExist(string email);
    }
}
