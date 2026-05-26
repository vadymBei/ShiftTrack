using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Interfaces;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Services;

public class ShiftService(
    IShiftRepository shiftRepository) : IShiftService
{
    public async Task<Shift> Create(ShiftToCreateDto shiftToCreateDto, CancellationToken cancellationToken)
    {
        await shiftRepository.CheckIfShiftExists(shiftToCreateDto.Code, cancellationToken);
        
        var shift = await shiftRepository.Create(shiftToCreateDto, cancellationToken);
        
        return shift;
    }
}