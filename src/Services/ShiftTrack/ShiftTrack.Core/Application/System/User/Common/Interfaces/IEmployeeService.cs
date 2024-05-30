using Data.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Domain.System.User.Employees.Entities;

namespace ShiftTrack.Core.Application.System.User.Common.Interfaces
{
    public interface IEmployeeService : IEntityServiceBase<Employee>
    {
        Task<Authentication.Models.User> RegisterUser(UserToRegisterDto dto, CancellationToken cancellationToken);
    }
}
