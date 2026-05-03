using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShiftHistories.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Constants;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Interfaces;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Services;

public class EmployeeShiftService(
    IShiftRepository shiftRepository,
    IEmployeeRepository employeeRepository,
    IEmployeeShiftRepository employeeShiftRepository,
    IEmployeeShiftHistoryRepository employeeShiftHistoryRepository) : IEmployeeShiftService
{
    public async Task<IEnumerable<EmployeeShift>> CreateEmployeeShifts(
        IEnumerable<EmployeeShiftToCreateDto> dtos,
        CancellationToken cancellationToken)
    {
        var shifts = (await shiftRepository.GetShiftsByIds(
            dtos.Select(x => x.ShiftId).Distinct(),
            cancellationToken)).ToDictionary(x => x.Id);

        var employeeIds = dtos
            .Select(x => x.EmployeeId)
            .Distinct();

        var employees = (await employeeRepository.GetEmployeesByIds(
            employeeIds,
            cancellationToken)).ToDictionary(x => x.Id);

        var startDate = dtos.Min(x => x.Date);
        var endDate = dtos.Max(x => x.Date);

        var existedEmployeeShifts = (await employeeShiftRepository.GetEmployeeShifts(
                new EmployeeShiftsFilterDto(
                    employeeIds,
                    startDate,
                    endDate),
                cancellationToken))
            .ToDictionary(x => new { x.Date.Date, x.EmployeeId });

        var employeeShiftToCreateList = new List<EmployeeShift>();
        var employeeShiftsToUpdate = new List<EmployeeShift>();
        var historyRecords = new List<EmployeeShiftHistory>();

        foreach (var dto in dtos)
        {
            if (!employees.TryGetValue(dto.EmployeeId, out var employee)
                || !shifts.TryGetValue(dto.ShiftId, out var shift))
            {
                continue;
            }

            existedEmployeeShifts.TryGetValue(new { dto.Date.Date, dto.EmployeeId }, out var existedEmployeeShift);

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

                employeeShiftsToUpdate.Add(existedEmployeeShift);
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

        if (employeeShiftToCreateList.Any())
        {
            await employeeShiftRepository.Create(employeeShiftToCreateList, cancellationToken);
        }

        if (employeeShiftsToUpdate.Any())
        {
            await employeeShiftRepository.Update(employeeShiftsToUpdate, cancellationToken);
        }

        if (historyRecords.Any())
        {
            await employeeShiftHistoryRepository.Create(historyRecords, cancellationToken);
        }

        return await employeeShiftRepository.GetEmployeeShifts(
            new EmployeeShiftsFilterDto(
                employeeIds,
                startDate,
                endDate,
                DisableTracking: true),
            cancellationToken);
    }

    public async Task<IEnumerable<EmployeeShift>> RestorePreviousEmployeeShifts(
        RestoreEmployeeShiftsDto dto,
        CancellationToken cancellationToken)
    {
        var employeeShifts = await employeeShiftRepository.GetEmployeeShifts(
            new EmployeeShiftsFilterDto(
                dto.EmployeeIds,
                dto.StartDate,
                dto.EndDate),
            cancellationToken);

        var employeeShiftHistory = (await employeeShiftHistoryRepository.GetByEmployeeShiftIds(
                employeeShifts.Select(x => x.Id),
                cancellationToken))
            .GroupBy(x => x.EmployeeShiftId)
            .ToDictionary(g => g.Key, g => g.ToList());

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

            if (employeeShiftHistory.TryGetValue(employeeShift.Id, out var history))
            {
                lastEmployeeShiftHistoryRecord = history
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
                var dismissedShift = await shiftRepository.GetShiftByCode(
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

        await employeeShiftRepository.Update(employeeShifts, cancellationToken);

        await employeeShiftHistoryRepository.Create(historyRecords, cancellationToken);

        return await employeeShiftRepository.GetEmployeeShifts(
            new EmployeeShiftsFilterDto(
                dto.EmployeeIds,
                dto.StartDate,
                dto.EndDate),
            cancellationToken);
    }
}