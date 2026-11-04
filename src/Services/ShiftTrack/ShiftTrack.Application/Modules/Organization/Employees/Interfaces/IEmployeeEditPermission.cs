using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Application.Modules.Organization.Employees.Interfaces;

/// <summary>Who may change an employee's data, department and hire date.</summary>
public interface IEmployeeEditPermission
{
    /// <summary>
    /// Throws unless the current user is a system administrator, or a unit director editing an employee of their scope
    /// (or without a department) and moving them only to a department of their scope.
    /// </summary>
    /// <param name="employee">As stored, with <see cref="Employee.Department"/>.</param>
    Task EnsureCanEdit(Employee employee, long? newDepartmentId, CancellationToken cancellationToken);
}
