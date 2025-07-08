using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Domain.System.Tokens.Models;
using ShiftTrack.Core.Domain.System.User.Employees.Entities;
using ShiftTrack.Core.Domain.System.User.Employees.Models;
using ShiftTrack.Data.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Common.Interfaces;

public interface IEmployeeService : IEntityServiceBase<Employee>
{
    Task<Authentication.Models.User> RegisterAuthUser(UserToRegisterDto dto, CancellationToken cancellationToken);
    Task<Authentication.Models.User> UpdateAuthUser(UserToUpdateDto dto, CancellationToken cancellationToken);
    Task<Token> ChangePassword(ChangeEmployeePasswordDto dto, CancellationToken cancellationToken);
    Task<CurrentUser> GetCurrentUser(CancellationToken cancellationToken);
    Task<Employee> GetByIntegrationId(string integrationId, CancellationToken cancellationToken);
}