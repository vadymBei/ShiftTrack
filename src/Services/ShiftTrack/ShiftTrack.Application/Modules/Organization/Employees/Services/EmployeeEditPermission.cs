using System.Net;
using Microsoft.Extensions.Logging;
using ShiftTrack.Application.Modules.Organization.Employees.Constants;
using ShiftTrack.Application.Modules.Organization.Employees.Exceptions;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Application.Modules.System.User.RoleScopes.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Constants;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Application.Modules.Organization.Employees.Services;

/// <summary>
/// The same rule as the administration pages: the system administrator edits everyone, a unit director —
/// the employees of the units and departments of their role bindings and the new employees without a department.
/// </summary>
public class EmployeeEditPermission(
    ICurrentUserService currentUserService,
    IEmployeeRoleChecker employeeRoleChecker,
    IRoleScopeService roleScopeService,
    IDepartmentRepository departmentRepository,
    ILogger<EmployeeEditPermission> logger) : IEmployeeEditPermission
{
    public async Task EnsureCanEdit(Employee employee, long? newDepartmentId, CancellationToken cancellationToken)
    {
        if (employeeRoleChecker.HasCurrentUserSysAdminRole())
            return;

        if (!employeeRoleChecker.HasCurrentUserUnitDirectorRole())
        {
            logger.LogWarning("{LogName} Employee {EditorId} without an administration role tried to edit employee {EmployeeId}",
                "EditEmployee", currentUserService.Employee.Id, employee.Id);

            throw Forbidden(EmployeeExceptionsLocalization.NO_PERMISSION_TO_EDIT_EMPLOYEES,
                nameof(EmployeeExceptionsLocalization.NO_PERMISSION_TO_EDIT_EMPLOYEES));
        }

        var scope = roleScopeService.GetCurrentScope(DefaultRolesCatalog.UNIT_DIRECTOR);

        if (employee.DepartmentId is not null && !scope.Covers(employee))
        {
            logger.LogWarning("{LogName} Unit director {EditorId} tried to edit employee {EmployeeId} of department {DepartmentId} out of their scope",
                "EditEmployee", currentUserService.Employee.Id, employee.Id, employee.DepartmentId);

            throw Forbidden(EmployeeExceptionsLocalization.EMPLOYEE_OUT_OF_SCOPE,
                nameof(EmployeeExceptionsLocalization.EMPLOYEE_OUT_OF_SCOPE));
        }

        if (newDepartmentId is null || newDepartmentId == employee.DepartmentId)
            return;

        var department = await departmentRepository.GetById(newDepartmentId.Value, cancellationToken);

        if (!scope.Covers(department.UnitId, department.Id))
        {
            logger.LogWarning("{LogName} Unit director {EditorId} tried to move employee {EmployeeId} to department {DepartmentId} out of their scope",
                "EditEmployee", currentUserService.Employee.Id, employee.Id, department.Id);

            throw Forbidden(EmployeeExceptionsLocalization.DEPARTMENT_OUT_OF_SCOPE,
                nameof(EmployeeExceptionsLocalization.DEPARTMENT_OUT_OF_SCOPE));
        }
    }

    private static EmployeeException Forbidden(string message, string type) =>
        new(message, type, HttpStatusCode.Forbidden);
}
