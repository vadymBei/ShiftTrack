using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Dtos;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;

public interface IEmployeeRoleService
{
    Task<EmployeeRole> Create(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken);
    Task Delete(long employeeRoleId, CancellationToken cancellationToken);
    Task<EmployeeRole> CreateSysAdminEmployeeRole(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken);
}