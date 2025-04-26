using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Core.Application.System.User.Common.Interfaces;

public interface IEmployeeRoleStrategy
{
    #region EmployeeRole

    Task<IEnumerable<EmployeeRole>> GetEmployeeRolesByEmployeeId(long employeeId, CancellationToken cancellationToken);
    Task<EmployeeRole> GetEmployeeRoleById(long employeeRoleId, CancellationToken cancellationToken);
    Task<EmployeeRole> CreateEmployeeRole(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken);
    Task DeleteEmployeeRole(long employeeRoleId, CancellationToken cancellationToken);

    #endregion

    #region EmployeeRoleUnit

    Task<EmployeeRoleUnit> GetEmployeeRoleUnitById(long employeeRoleUnitId, CancellationToken cancellationToken);
    Task<IEnumerable<EmployeeRoleUnit>> GetEmployeeRoleUnitsByEmployeeRoleId(long employeeRoleId, CancellationToken cancellationToken);
    Task<EmployeeRoleUnit> CreateEmployeeRoleUnit(EmployeeRoleUnitToCreateDto dto, CancellationToken cancellationToken);
    Task DeleteEmployeeRoleUnit(long employeeUnitId, CancellationToken cancellationToken);

    #endregion

    #region EmployeeRoleUnitDepartment
    
    Task<IEnumerable<EmployeeRoleUnitDepartment>> CreateEmployeeRoleUnitDepartments(
        EmployeeRoleUnitDepartmentsToCreateDto dto, CancellationToken cancellationToken);

    Task<IEnumerable<EmployeeRoleUnitDepartment>> GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitId(
        long employeeRoleUnitId,
        CancellationToken cancellationToken);
    
    Task DeleteEmployeeRoleUnitDepartment(long employeeRoleUnitDepartmentId, CancellationToken cancellationToken);
    
    #endregion
}