using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Employees.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Dtos;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Common.Services;

public class EmployeeShiftService(
    IShiftService shiftService,
    IEmployeeService employeeService,
    IApplicationDbContext applicationDbContext) : IEmployeeShiftService
{
    public async Task<IEnumerable<EmployeeShift>> GetEmployeeShifts(EmployeeShiftsFilterDto filter, CancellationToken cancellationToken)
    {
        var employeeShifts = await applicationDbContext.EmployeeShifts
            .Include(x => x.Shift)
            .Where(x => filter.EmployeeIds.Contains(x.EmployeeId)
                        && x.Date.Date >= filter.StartDate.Date
                        && x.Date.Date <= filter.EndDate.Date)
            .ToListAsync(cancellationToken);

        return employeeShifts;
    }

    public async Task<IEnumerable<EmployeeShift>> CreateEmployeeShifts(
        IEnumerable<EmployeeShiftToCreateDto> dtos,
        CancellationToken cancellationToken)
    {
        var shifts = await shiftService.GetShiftsByIds(
            dtos.Select(x => x.ShiftId).Distinct(),
            cancellationToken);

        var employeeIds = dtos
            .Select(x => x.EmployeeId)
            .Distinct();

        var employees = await employeeService.GetEmployeesByIds(
            employeeIds,
            cancellationToken);
        
        var startDate = dtos.Min(x => x.Date);
        var endDate = dtos.Max(x => x.Date);

        var employeeShiftsFilterDto = new EmployeeShiftsFilterDto(
            employeeIds,
            startDate,
            endDate);
        
        var existedEmployeeShifts = await GetEmployeeShifts(
            employeeShiftsFilterDto,
            cancellationToken);
        
        var employeeShiftToCreateList = new List<EmployeeShift>();

        foreach (var dto in dtos)
        {
            var employee = employees.FirstOrDefault(x => x.Id == dto.EmployeeId);

            var shift = shifts.FirstOrDefault(x => x.Id == dto.ShiftId);

            if (shift is null
                || employee is null)
            {
                continue;
            }

            var existedEmployeeShift = existedEmployeeShifts
                .FirstOrDefault(x => x.Date.Date == dto.Date.Date
                                     && x.EmployeeId == dto.EmployeeId);

            if (existedEmployeeShift is not null)
            {
                existedEmployeeShift.ShiftId = shift.Id;
                existedEmployeeShift.StartTime = shift.StartTime;
                existedEmployeeShift.EndTime = shift.EndTime;
            }
            else
            {
                employeeShiftToCreateList.Add(
                    new EmployeeShift()
                    {
                        Date = dto.Date,
                        ShiftId = dto.ShiftId,
                        EmployeeId = dto.EmployeeId,
                        StartTime = shift.StartTime,
                        EndTime = shift.EndTime
                    });
            }
        }

        applicationDbContext.EmployeeShifts.AddRange(employeeShiftToCreateList);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return await GetEmployeeShifts(employeeShiftsFilterDto, cancellationToken);
    }
}