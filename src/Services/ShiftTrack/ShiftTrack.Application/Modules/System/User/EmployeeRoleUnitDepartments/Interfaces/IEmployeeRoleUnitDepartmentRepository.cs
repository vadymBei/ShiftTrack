using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Dtos;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Interfaces;

public interface IEmployeeRoleUnitDepartmentRepository
{
    Task<IEnumerable<EmployeeRoleUnitDepartment>> Create(EmployeeRoleUnitDepartmentsToCreateDto dto, CancellationToken cancellationToken);
    Task CheckIfExists(EmployeeRoleUnitDepartmentsToCreateDto dto, CancellationToken cancellationToken);
    Task<IEnumerable<EmployeeRoleUnitDepartment>> GetListByEmployeeRoleUnitId(long employeeRoleUnitId, CancellationToken cancellationToken);
    Task<EmployeeRoleUnitDepartment> GetById(long id, CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
}