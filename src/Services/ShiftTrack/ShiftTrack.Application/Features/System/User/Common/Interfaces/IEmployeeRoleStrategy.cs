using ShiftTrack.Application.Features.System.User.Common.Dtos;
using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Features.System.User.Common.Interfaces;

public interface IEmployeeRoleStrategy
{
    #region EmployeeRole

    Task<EmployeeRole> CreateEmployeeRole(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken);
    Task DeleteEmployeeRole(long employeeRoleId, CancellationToken cancellationToken);

    #endregion

    #region EmployeeRoleUnit

    Task<EmployeeRoleUnit> CreateEmployeeRoleUnit(EmployeeRoleUnitToCreateDto dto, CancellationToken cancellationToken);
    Task DeleteEmployeeRoleUnit(long employeeUnitId, CancellationToken cancellationToken);

    #endregion

    #region EmployeeRoleUnitDepartment
    
    Task<IEnumerable<EmployeeRoleUnitDepartment>> CreateEmployeeRoleUnitDepartments(
        EmployeeRoleUnitDepartmentsToCreateDto dto, CancellationToken cancellationToken);
    
    Task DeleteEmployeeRoleUnitDepartment(long employeeRoleUnitDepartmentId, CancellationToken cancellationToken);
    
    #endregion
}