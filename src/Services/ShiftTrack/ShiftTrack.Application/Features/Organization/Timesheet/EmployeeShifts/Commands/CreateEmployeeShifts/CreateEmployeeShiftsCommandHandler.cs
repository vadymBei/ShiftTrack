using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Timesheet.EmployeeShifts.Commands.CreateEmployeeShifts;

public class CreateEmployeeShiftsCommandHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<CreateEmployeeShiftsCommand, IEnumerable<EmployeeShiftVm>>
{
    public async Task<IEnumerable<EmployeeShiftVm>> Handle(CreateEmployeeShiftsCommand request,
        CancellationToken cancellationToken = default)
    {
        var shifts = await GetShifts(request.Dtos.Select(x => x.ShiftId), cancellationToken);

        var employees = await GetEmployees(request.Dtos.Select(x => x.EmployeeId), cancellationToken);

        var existedEmployeeShifts = await GetEmployeeShifts(
            request.Dtos.Select(x => x.EmployeeId).Distinct(),
            request.Dtos.Min(x => x.Date),
            request.Dtos.Max(x => x.Date),
            cancellationToken);

        var employeeShiftToCreateList = new List<EmployeeShift>();

        foreach (var dto in request.Dtos)
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

        var employeeShifts = await GetEmployeeShifts(
            request.Dtos.Select(x => x.EmployeeId).Distinct(),
            request.Dtos.Min(x => x.Date),
            request.Dtos.Max(x => x.Date),
            cancellationToken);

        return mapper.Map<IEnumerable<EmployeeShiftVm>>(employeeShifts);
    }

    private async Task<IEnumerable<Shift>> GetShifts(
        IEnumerable<long> shiftIds,
        CancellationToken cancellationToken)
    {
        var shifts = await applicationDbContext.Shifts
            .AsNoTracking()
            .Where(x => shiftIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        return shifts;
    }

    private async Task<IEnumerable<Employee>> GetEmployees(
        IEnumerable<long> employeeIds,
        CancellationToken cancellationToken)
    {
        var employees = await applicationDbContext.Employees
            .AsNoTracking()
            .Where(x => employeeIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        return employees;
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
}