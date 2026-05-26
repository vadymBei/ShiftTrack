using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Interfaces;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Infrastructure.Common.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Infrastructure.Modules.Organization.Timesheet.Shifts.Repositories;

public class ShiftRepository(
    IApplicationDbContext applicationDbContext) : IShiftRepository
{
    public async Task<Shift> GetById(long id, CancellationToken cancellationToken)
    {
        var shift = await applicationDbContext.Shifts
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                    ?? throw new EntityNotFoundException(typeof(Shift), id);

        return shift;
    }

    public async Task<IEnumerable<Shift>> GetAll(CancellationToken cancellationToken)
    {
        var shifts = await applicationDbContext.Shifts
            .AsNoTracking()
            .OrderBy(x => x.Code)
            .ToListAsync(cancellationToken);
        
        return shifts;
    }

    public async Task<IEnumerable<Shift>> GetShiftsByIds(IEnumerable<long> shiftIds,
        CancellationToken cancellationToken)
    {
        var shifts = await applicationDbContext.Shifts
            .AsNoTracking()
            .Where(x => shiftIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        return shifts;
    }

    public async Task<Shift> GetShiftByCode(string code, CancellationToken cancellationToken)
    {
        var shift = await applicationDbContext.Shifts
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Code == code, cancellationToken)
                    ?? throw new EntityNotFoundException($"Shift with code '{code}' not found");

        return shift;
    }

    public async Task CheckIfShiftExists(string code, CancellationToken cancellationToken)
    {
        var isShiftExist = await applicationDbContext.Shifts
            .AsTracking()
            .AnyAsync(x => x.Code == code, cancellationToken);

        if (isShiftExist)
        {
            throw new Exception($"Shift already exist with this code {code}.");
        }
    }

    public async Task<Shift> Create(ShiftToCreateDto shiftToCreateDto, CancellationToken cancellationToken)
    {
        var shift = new Shift()
        {
            Code = shiftToCreateDto.Code,
            Description = shiftToCreateDto.Description,
            Type = shiftToCreateDto.Type,
            Color = shiftToCreateDto.Color
        };

        if (shiftToCreateDto.StartTime.HasValue
            && shiftToCreateDto.EndTime.HasValue)
        {
            shift.StartTime = shiftToCreateDto.StartTime;
            shift.EndTime = shiftToCreateDto.EndTime;
            shift.WorkHours = shiftToCreateDto.EndTime.Value - shiftToCreateDto.StartTime.Value;
        }

        await applicationDbContext.Shifts.AddAsync(shift, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return shift;
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var shift = await applicationDbContext.Shifts
                        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                    ?? throw new EntityNotFoundException(typeof(Shift), id);

        applicationDbContext.Shifts.Remove(shift);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Shift> Update(ShiftToUpdateDto shiftToUpdateDto, CancellationToken cancellationToken)
    {
        var shift = await applicationDbContext.Shifts
                        .FirstOrDefaultAsync(x => x.Id == shiftToUpdateDto.Id, cancellationToken)
                    ?? throw new EntityNotFoundException(typeof(Shift), shiftToUpdateDto.Id);

        shift.Code = shiftToUpdateDto.Code;
        shift.Description = shiftToUpdateDto.Description;
        shift.Color = shiftToUpdateDto.Color;
        shift.Type = shiftToUpdateDto.Type;
        shift.StartTime = shiftToUpdateDto.StartTime;
        shift.EndTime = shiftToUpdateDto.EndTime;

        if (shiftToUpdateDto.StartTime.HasValue
            && shiftToUpdateDto.EndTime.HasValue)
        {
            shift.WorkHours = shiftToUpdateDto.EndTime.Value - shiftToUpdateDto.StartTime.Value;
        }
        else
        {
            shift.WorkHours = null;
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return shift;
    }
}