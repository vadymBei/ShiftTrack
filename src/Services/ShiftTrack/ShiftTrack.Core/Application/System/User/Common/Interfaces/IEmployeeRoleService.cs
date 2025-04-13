using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Core.Application.System.User.Common.Interfaces;

public interface IEmployeeRoleService
{
    Task<EmployeeRole> CreateEmployeeRole(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken);
    Task DeleteEmployeeRole(long employeeRoleId, CancellationToken cancellationToken);
    Task<EmployeeRole> GetEmployeeRoleById(long employeeRoleId, CancellationToken cancellationToken);
    Task<IEnumerable<EmployeeRole>> GetEmployeeRolesByEmployeeId(long employeeId, CancellationToken cancellationToken);
}