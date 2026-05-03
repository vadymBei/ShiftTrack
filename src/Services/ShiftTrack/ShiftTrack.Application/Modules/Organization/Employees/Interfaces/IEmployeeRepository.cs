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
    Task<IEnumerable<Employee>> GetEmployeesByIds(IEnumerable<long> ids, CancellationToken cancellationToken);
    Task<IEnumerable<Employee>> GetEmployees(EmployeesFilterDto filter, CancellationToken cancellationToken);
    Task<Employee> Update(EmployeeToUpdateDto employeeToUpdateDto, CancellationToken cancellationToken);
    Task UpdateEmployeePhoto(UploadEmployeePhotoDto uploadEmployeePhotoDto, CancellationToken cancellationToken);
}