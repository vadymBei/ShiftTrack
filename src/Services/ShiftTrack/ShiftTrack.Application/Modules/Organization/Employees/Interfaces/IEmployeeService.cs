using ShiftTrack.Application.Modules.Organization.Employees.Dtos;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Application.Modules.Organization.Employees.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetEmployees(EmployeesFilterDto filter, CancellationToken cancellationToken);
    Task<Employee> Update(EmployeeToUpdateDto employeeToUpdateDto, CancellationToken cancellationToken);
}