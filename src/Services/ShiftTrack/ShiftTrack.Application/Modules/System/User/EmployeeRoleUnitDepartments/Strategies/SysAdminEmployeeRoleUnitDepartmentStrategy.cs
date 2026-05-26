using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Interfaces;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Strategies;

public class SysAdminEmployeeRoleUnitDepartmentStrategy(
    IEmployeeRoleUnitDepartmentRepository employeeRoleUnitDepartmentRepository) : IEmployeeRoleUnitDepartmentStrategy
{
    public async Task<IEnumerable<EmployeeRoleUnitDepartment>> Create(
        EmployeeRoleUnitDepartmentsToCreateDto dto,
        CancellationToken cancellationToken)
    {
        var employeeRoleUnitDepartments = await employeeRoleUnitDepartmentRepository.Create(
            dto,
            cancellationToken);

        return employeeRoleUnitDepartments;
    }

    public Task Delete(long employeeRoleUnitDepartmentId, CancellationToken cancellationToken)
    {
        return employeeRoleUnitDepartmentRepository.Delete(employeeRoleUnitDepartmentId, cancellationToken);
    }
}