using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Dtos;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Interfaces;

public interface IEmployeeRoleUnitDepartmentStrategy
{
    Task<IEnumerable<EmployeeRoleUnitDepartment>> Create(EmployeeRoleUnitDepartmentsToCreateDto dto, CancellationToken cancellationToken);
    Task Delete(long employeeRoleUnitDepartmentId, CancellationToken cancellationToken);

}