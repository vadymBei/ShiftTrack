using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Organization.Employees.Dtos;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;
using ShiftTrack.Infrastructure.Common.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Infrastructure.Modules.Organization.Employees.Repositories;

public class EmployeeRepository(
    IApplicationDbContext applicationDbContext) : IEmployeeRepository
{
    public async Task<Employee> Create(EmployeeToCreateDto employeeToCreateDto, CancellationToken cancellationToken)
    {
        var employee = new Employee
        {
            IntegrationId = employeeToCreateDto.IntegrationId,
            Name = employeeToCreateDto.Name,
            Surname = employeeToCreateDto.Surname,
            Patronymic = employeeToCreateDto.Patronymic,
            Email = employeeToCreateDto.Email,
            PhoneNumber = employeeToCreateDto.PhoneNumber,
            Gender = employeeToCreateDto.Gender
        };

        applicationDbContext.Employees.Add(employee);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return await GetById(employee.Id, cancellationToken);
    }

    public async Task<Employee> GetById(long id, CancellationToken cancellationToken)
    {
        var employee = await applicationDbContext.Employees
                           .AsNoTracking()
                           .Include(x => x.Department)
                           .ThenInclude(x => x.Unit)
                           .Include(x => x.Position)
                           .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                       ?? throw new EntityNotFoundException(typeof(Employee), id);

        return employee;
    }

    public async Task<Employee> GetByIntegrationId(string integrationId, CancellationToken cancellationToken)
    {
        var employee = await applicationDbContext.Employees
            .AsNoTracking()
            .Include(x => x.Position)
            .Include(x => x.Department)
            .ThenInclude(x => x.Unit)
            .Include(x => x.EmployeeRoles)
            .ThenInclude(x => x.Role)
            .Include(x => x.EmployeeRoles)
            .ThenInclude(x => x.Units)
            .ThenInclude(x => x.Departments)
            .ThenInclude(x => x.Department)
            .FirstOrDefaultAsync(x => x.IntegrationId == integrationId, cancellationToken);

        return employee;
    }

    public async Task<Employee> GetByPhoneNumber(string phoneNumber, CancellationToken cancellationToken)
    {
        var employee = await applicationDbContext.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber, cancellationToken);

       return employee;
    }

    public async Task<Employee> UpdateVacationDaysBalance(long employeeId, int vacationDaysBalance,
        CancellationToken cancellationToken)
    {
        var employee = await applicationDbContext.Employees
                           .FirstOrDefaultAsync(x => x.Id == employeeId, cancellationToken)
                       ?? throw new EntityNotFoundException(typeof(Employee), employeeId);

        employee.VacationDaysBalance = vacationDaysBalance;

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return employee;
    }

    public async Task<IEnumerable<Employee>> GetListByIds(IEnumerable<long> ids,
        CancellationToken cancellationToken)
    {
        var employees = await applicationDbContext.Employees
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        return employees;
    }

    public async Task<IEnumerable<Employee>> GetFiltered(EmployeesFilterDto filter,
        CancellationToken cancellationToken)
    {
        var employeeQuery = applicationDbContext.Employees
            .Include(x => x.Department)
            .ThenInclude(x => x.Unit)
            .Include(x => x.Position)
            .AsQueryable();

        if (filter.UnitId is not null)
        {
            employeeQuery = employeeQuery
                .Where(x => x.Department.UnitId == filter.UnitId);
        }

        if (filter.DepartmentId is not null)
        {
            employeeQuery = employeeQuery
                .Where(x => x.DepartmentId == filter.DepartmentId);
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchPattern))
        {
            employeeQuery = employeeQuery
                .Where(x => EF.Functions.Like(
                    x.Surname.ToLower() + " " + x.Name.ToLower() + " " + x.Patronymic.ToLower(),
                    $"%{filter.SearchPattern.ToLower()}%"));
        }

        var employees = await employeeQuery
            .AsNoTracking()
            .OrderBy(x => x.Surname)
            .ThenBy(x => x.Name)
            .ThenBy(x => x.Patronymic)
            .ToListAsync(cancellationToken);

        return employees;
    }

    public async Task<Employee> Update(EmployeeToUpdateDto employeeToUpdateDto, CancellationToken cancellationToken)
    {
        var employee = await applicationDbContext.Employees
                           .FirstOrDefaultAsync(x => x.Id == employeeToUpdateDto.Id, cancellationToken)
                       ?? throw new EntityNotFoundException(typeof(Employee), employeeToUpdateDto.Id);

        employee.Name = employeeToUpdateDto.Name;
        employee.Surname = employeeToUpdateDto.Surname;
        employee.Patronymic = employeeToUpdateDto.Patronymic;
        employee.Email = employeeToUpdateDto.Email;
        employee.DateOfBirth = employeeToUpdateDto.DateOfBirth;
        employee.Gender = employeeToUpdateDto.Gender;
        employee.DepartmentId = employeeToUpdateDto.DepartmentId;
        employee.PositionId = employeeToUpdateDto.PositionId;
        employee.PhoneNumber = employeeToUpdateDto.PhoneNumber;

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return employee;
    }

    public async Task UpdatePhoto(UploadEmployeePhotoDto uploadEmployeePhotoDto, CancellationToken cancellationToken)
    {
        var employee = await applicationDbContext.Employees
                           .FindAsync([uploadEmployeePhotoDto.EmployeeId], cancellationToken)
                       ?? throw new EntityNotFoundException(typeof(Employee), uploadEmployeePhotoDto.EmployeeId);
        
        employee.PhotoFullName = uploadEmployeePhotoDto.PhotoUrl;
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}