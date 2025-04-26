using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Core.Application.System.User.Common.Interfaces;

public interface IEmployeeRoleService
{
    #region EmployeeRole

    Task<EmployeeRole> CreateEmployeeRole(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken);
    Task DeleteEmployeeRole(long employeeRoleId, CancellationToken cancellationToken);
    Task<EmployeeRole> GetEmployeeRoleById(long employeeRoleId, CancellationToken cancellationToken);
    Task<IEnumerable<EmployeeRole>> GetEmployeeRolesByEmployeeId(long employeeId, CancellationToken cancellationToken);

    #endregion

    #region EmployeeRoleUnit

    Task<EmployeeRoleUnit> CreateEmployeeRoleUnit(EmployeeRoleUnitToCreateDto dto, CancellationToken cancellationToken);
    Task<EmployeeRoleUnit> GetEmployeeRoleUnitById(long employeeRoleUnitId, CancellationToken cancellationToken);
    Task<IEnumerable<EmployeeRoleUnit>> GetEmployeeRoleUnitsByEmployeeRoleId(long employeeRoleId, CancellationToken cancellationToken);
    Task DeleteEmployeeRoleUnit(long employeeRoleUnitId, CancellationToken cancellationToken);

    #endregion

    #region EmployeeRoleUnitDepartment

    Task<EmployeeRoleUnitDepartment> CreateEmployeeRoleUnitDepartments(EmployeeRoleUnitDepartmentsToCreateDto dto, CancellationToken cancellationToken);
    Task<IEnumerable<EmployeeRoleUnitDepartment>> GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitId(long employeeRoleUnitId, CancellationToken cancellationToken);
    Task DeleteEmployeeRoleUnitDepartment(long employeeRoleUnitDepartmentId, CancellationToken cancellationToken);

    #endregion
}