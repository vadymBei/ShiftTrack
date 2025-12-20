using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Common.Constants;
using ShiftTrack.Application.Features.Organization.Employees.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Dtos;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Common.Services;

public class EmployeeShiftService(
    IShiftService shiftService,
    IEmployeeService employeeService,
    IApplicationDbContext applicationDbContext,
    IEmployeeShiftHistoryService employeeShiftHistoryService) : IEmployeeShiftService
{
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

        var existedEmployeeShifts = await GetEmployeeShifts(
            employeeIds,
            startDate,
            endDate,
            cancellationToken);

        var employeeShiftToCreateList = new List<EmployeeShift>();
        var historyRecords = new List<EmployeeShiftHistory>();

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
                historyRecords.Add(new EmployeeShiftHistory
                {
                    EmployeeShiftId = existedEmployeeShift.Id,
                    PreviousShiftId = existedEmployeeShift.ShiftId,
                    PreviousStartTime = existedEmployeeShift.StartTime,
                    PreviousEndTime = existedEmployeeShift.EndTime,
                    NewShiftId = shift.Id,
                    NewStartTime = shift.StartTime,
                    NewEndTime = shift.EndTime
                });

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
                        EndTime = shift.EndTime,
                        History =
                        [
                            new EmployeeShiftHistory()
                            {
                                NewShiftId = shift.Id,
                                NewStartTime = shift.StartTime,
                                NewEndTime = shift.EndTime
                            }
                        ]
                    });
            }
        }

        applicationDbContext.EmployeeShifts.AddRange(employeeShiftToCreateList);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        await employeeShiftHistoryService.Create(historyRecords, cancellationToken);

        return await GetEmployeeShifts(
            employeeIds,
            startDate,
            endDate,
            cancellationToken);
    }

    private async Task<IEnumerable<EmployeeShift>> GetEmployeeShifts(
        IEnumerable<long> employeeIds,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken)
    {
        var employeeShifts = await applicationDbContext.EmployeeShifts
            .Include(x => x.Shift)
            .Where(x => employeeIds.Contains(x.EmployeeId)
                        && x.Date.Date >= startDate.Date
                        && x.Date.Date <= endDate.Date)
            .ToListAsync(cancellationToken);

        return employeeShifts;
    }

    public async Task<IEnumerable<EmployeeShift>> RestorePreviousEmployeeShifts(
        RestoreEmployeeShiftsDto dto,
        CancellationToken cancellationToken)
    {
        var employeeShifts = await GetEmployeeShifts(
            dto.EmployeeIds,
            dto.StartDate,
            dto.EndDate,
            cancellationToken);

        var employeeShiftHistory = await employeeShiftHistoryService.GetByEmployeeShiftIds(
            employeeShifts.Select(x => x.Id),
            cancellationToken);

        var historyRecords = new List<EmployeeShiftHistory>();

        foreach (var employeeShift in employeeShifts)
        {
            EmployeeShiftHistory lastEmployeeShiftHistoryRecord = null;
            var historyRecord = new EmployeeShiftHistory()
            {
                EmployeeShiftId = employeeShift.Id,
                PreviousShiftId = employeeShift.ShiftId,
                PreviousStartTime = employeeShift.StartTime,
                PreviousEndTime = employeeShift.EndTime
            };

            if (employeeShiftHistory.Any())
            {
                lastEmployeeShiftHistoryRecord = employeeShiftHistory
                    .Where(x => x.EmployeeShiftId == employeeShift.Id)
                    .OrderBy(x => x.CreatedAt)
                    .LastOrDefault();
            }

            if (lastEmployeeShiftHistoryRecord?.PreviousShiftId != null)
            {
                employeeShift.StartTime = lastEmployeeShiftHistoryRecord.PreviousStartTime;
                employeeShift.EndTime = lastEmployeeShiftHistoryRecord.PreviousEndTime;
                employeeShift.ShiftId = (long)lastEmployeeShiftHistoryRecord.PreviousShiftId;
            }
            else
            {
                var dismissedShift = await shiftService.GetShiftByCode(
                    ShiftCodes.Dismissed,
                    cancellationToken);

                employeeShift.ShiftId = dismissedShift.Id;
                employeeShift.StartTime = dismissedShift.StartTime;
                employeeShift.EndTime = dismissedShift.EndTime;
            }

            historyRecord.NewShiftId = employeeShift.ShiftId;
            historyRecord.NewStartTime = employeeShift.StartTime;
            historyRecord.NewEndTime = employeeShift.EndTime;

            historyRecords.Add(historyRecord);
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        await employeeShiftHistoryService.Create(historyRecords, cancellationToken);

        return employeeShifts;
    }
}