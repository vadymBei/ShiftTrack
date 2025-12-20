using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Employees.Common.Dtos;
using ShiftTrack.Application.Features.Organization.Employees.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Dtos;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Models;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Common.Services;

public class TimesheetService(
    IEmployeeService employeeService,
    IDepartmentService departmentService,
    IApplicationDbContext applicationDbContext) : ITimesheetService
{
    public async Task<UnitTimesheet> GetTimesheet(TimesheetDto dto, CancellationToken cancellationToken)
    {
        var startDate = new DateTime(dto.Period.Year, dto.Period.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var department = await departmentService.GetById(dto.DepartmentId, cancellationToken);

        var timesheet = new UnitTimesheet()
        {
            StartDate = startDate,
            EndDate = endDate,
            Department = department
        };

        var employees = await employeeService.GetEmployees(
            new EmployeesFilterDto(
                null,
                dto.DepartmentId),
            cancellationToken);

        var employeeIds = employees.Select(x => x.Id);

        var employeeShifts = await applicationDbContext.EmployeeShifts
            .AsNoTracking()
            .Include(x => x.Shift)
            .Where(x => employeeIds.Contains(x.EmployeeId)
                        && x.Date >= startDate
                        && x.Date <= endDate)
            .ToListAsync(cancellationToken);

        foreach (var employee in employees)
        {
            var employeeTimesheet = new EmployeeTimesheet()
            {
                Employee = employee,
                EmployeeShifts = employeeShifts.Where(x => x.EmployeeId == employee.Id)
            };

            timesheet.EmployeeTimesheets.Add(employeeTimesheet);
        }

        timesheet.EmployeeTimesheets = timesheet.EmployeeTimesheets
            .OrderBy(x => x.Employee.FullName)
            .ToList();

        return timesheet;
    }
}