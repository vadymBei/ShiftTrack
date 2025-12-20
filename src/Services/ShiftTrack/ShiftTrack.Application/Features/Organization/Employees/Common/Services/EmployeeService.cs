using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Employees.Common.Dtos;
using ShiftTrack.Application.Features.Organization.Employees.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.Interfaces;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Organization.Employees.Common.Services;

public class EmployeeService(
    IDepartmentService departmentService,
    IApplicationDbContext applicationDbContext) : IEmployeeService
{
    public async Task<Employee> GetById(object id, CancellationToken cancellationToken)
    {
        var employeeId = (long)id;

        var employee = await applicationDbContext.Employees
            .AsNoTracking()
            .Include(x => x.Department)
            .ThenInclude(x => x.Unit)
            .Include(x => x.Position)
            .FirstOrDefaultAsync(x => x.Id == employeeId, cancellationToken);

        if (employee == null)
        {
            throw new EntityNotFoundException(typeof(Employee), employeeId);
        }

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

    public async Task<IEnumerable<Employee>> GetEmployeesByIds(IEnumerable<long> ids,
        CancellationToken cancellationToken)
    {
        var employees = await applicationDbContext.Employees
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        return employees;
    }

    public async Task<IEnumerable<Employee>> GetEmployees(
        EmployeesFilterDto filter,
        CancellationToken cancellationToken)
    {
        var employeeQuery = applicationDbContext.Employees
            .Include(x => x.Department)
            .ThenInclude(x => x.Unit)
            .Include(x => x.Position)
            .AsQueryable();

        if (filter.DepartmentId is not null)
        {
            await departmentService.GetById(filter.DepartmentId, cancellationToken);

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
}