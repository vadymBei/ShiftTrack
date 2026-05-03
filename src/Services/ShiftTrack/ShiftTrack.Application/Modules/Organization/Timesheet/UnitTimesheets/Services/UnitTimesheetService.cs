using ShiftTrack.Application.Modules.Organization.Employees.Dtos;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Interfaces;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Models;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Services;

public class UnitTimesheetService(
    IEmployeeService employeeService,
    IDepartmentRepository departmentRepository,
    IEmployeeShiftRepository employeeShiftRepository) : IUnitTimesheetService
{
    public async Task<UnitTimesheet> GetTimesheet(UnitTimesheetDto dto, CancellationToken cancellationToken)
    {
        var startDate = new DateTime(dto.Period.Year, dto.Period.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var department = await departmentRepository.GetById(dto.DepartmentId, cancellationToken);

        var timesheet = new UnitTimesheet()
        {
            StartDate = startDate,
            EndDate = endDate,
            Department = department
        };

        var employees = await employeeService.GetEmployees(
            new EmployeesFilterDto(
                null,
                null,
                dto.DepartmentId),
            cancellationToken);

        var employeeIds = employees.Select(x => x.Id);

        var employeeShifts = await employeeShiftRepository.GetEmployeeShifts(
            new EmployeeShiftsFilterDto(employeeIds, startDate, endDate),
            cancellationToken);

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