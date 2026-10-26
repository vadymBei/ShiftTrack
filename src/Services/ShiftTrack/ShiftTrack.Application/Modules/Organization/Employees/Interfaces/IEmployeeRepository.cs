using ShiftTrack.Application.Modules.Organization.Employees.Models;
using ShiftTrack.Application.Modules.Organization.Employees.Dtos;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Application.Modules.Organization.Employees.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee> Create(EmployeeToCreateDto employeeToCreateDto, CancellationToken cancellationToken);
    Task<Employee> GetById(long id, CancellationToken cancellationToken);
    Task<Employee> GetByIntegrationId(string integrationId, CancellationToken cancellationToken);
    Task<Employee> GetByPhoneNumber(string phoneNumber, CancellationToken cancellationToken);
    Task<Employee> UpdateVacationDaysBalance(long employeeId, int vacationDaysBalance, CancellationToken cancellationToken);
    Task<IEnumerable<Employee>> GetListByIds(IEnumerable<long> ids, CancellationToken cancellationToken);
    Task<IEnumerable<Employee>> GetFiltered(EmployeesFilterDto filter, CancellationToken cancellationToken);
    Task<Employee> Update(EmployeeToUpdateDto employeeToUpdateDto, CancellationToken cancellationToken);
    Task UpdatePhoto(UploadEmployeePhotoDto uploadEmployeePhotoDto, CancellationToken cancellationToken);
    Task<IReadOnlyList<EmployeeServiceStart>> GetServiceStarts(CancellationToken cancellationToken);
    Task UpdateVacationDaysBalances(IReadOnlyDictionary<long, int> balances, CancellationToken cancellationToken);
    /// <summary>
    /// Employees with the role whose bindings cover any of the departments:
    /// a binding to the department's unit without departments, or to the department itself.
    /// </summary>
    Task<IReadOnlyCollection<long>> GetIdsByRoleScope(string roleName, IReadOnlyCollection<long> departmentIds,
        CancellationToken cancellationToken);
}