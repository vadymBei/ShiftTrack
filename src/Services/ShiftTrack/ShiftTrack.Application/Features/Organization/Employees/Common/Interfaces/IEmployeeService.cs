using ShiftTrack.Application.Features.System.Auth.Common.Dtos;
using ShiftTrack.Application.Features.System.User.Common.Dtos;
using ShiftTrack.Data.Interfaces;
using ShiftTrack.Domain.Features.System.Auth.Models;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;

namespace ShiftTrack.Application.Features.Organization.Employees.Common.Interfaces;

public interface IEmployeeService : IEntityServiceBase<Employee>
{
    Task<Authentication.Models.User> RegisterAuthUser(UserToRegisterDto dto, CancellationToken cancellationToken);
    Task<Authentication.Models.User> UpdateAuthUser(UserToUpdateDto dto, CancellationToken cancellationToken);
    Task<Token> ChangePassword(ChangePasswordDto dto, CancellationToken cancellationToken);
    Task<Employee> GetByIntegrationId(string integrationId, CancellationToken cancellationToken);
    Task<Employee> UpdateVacationDaysBalance(long employeeId, int vacationDaysBalance, CancellationToken cancellationToken);
}