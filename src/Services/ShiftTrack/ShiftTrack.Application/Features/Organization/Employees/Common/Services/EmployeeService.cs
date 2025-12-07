using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Employees.Common.Interfaces;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Organization.Employees.Common.Services;

public class EmployeeService(
    IApplicationDbContext applicationDbContext) : IEmployeeService
{
    public async Task<Employee> GetById(object id, CancellationToken cancellationToken)
    {
        long employeeId = (long)id;

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
}