using ShiftTrack.Application.Modules.Organization.Employees.Common.Dtos;
using ShiftTrack.Data.Interfaces;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Application.Modules.Organization.Employees.Common.Interfaces;

public interface IEmployeeService : IEntityServiceBase<Employee>
{
    Task<Employee> GetByIntegrationId(string integrationId, CancellationToken cancellationToken);
    Task<Employee> UpdateVacationDaysBalance(long employeeId, int vacationDaysBalance, CancellationToken cancellationToken);
    Task<IEnumerable<Employee>> GetEmployeesByIds(IEnumerable<long> ids, CancellationToken cancellationToken);
    Task<IEnumerable<Employee>> GetEmployees(EmployeesFilterDto filter, CancellationToken cancellationToken);
}