using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;
using ShiftTrack.Infrastructure.Common.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Infrastructure.Modules.Organization.Structure.Units.Repositories;

public class UnitRepository(
    IApplicationDbContext applicationDbContext) : IUnitRepository
{
    public async Task<Unit> GetById(long id, CancellationToken cancellationToken)
    {
        var unit = await applicationDbContext.Units
                       .AsNoTracking()
                       .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                   ?? throw new EntityNotFoundException(typeof(Unit), id);

        return unit;
    }

    public async Task<IEnumerable<Unit>> GetAll(CancellationToken cancellationToken)
    {
        return await applicationDbContext.Units
            .AsNoTracking()
            .Include(x => x.Author)
            .Include(x => x.Modifier)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Unit>> GetByIds(IEnumerable<long> ids, CancellationToken cancellationToken)
    {
        return await applicationDbContext.Units
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<Unit> Create(UnitToCreateDto unitToCreateDto, CancellationToken cancellationToken)
    {
        var unit = new Unit()
        {
            Name = unitToCreateDto.Name,
            Description = unitToCreateDto.Description,
            Code = unitToCreateDto.Code
        };

        await applicationDbContext.Units.AddAsync(unit, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return unit;
    }

    public async Task<Unit> Update(UnitToUpdateDto unitToUpdateDto, CancellationToken cancellationToken)
    {
        var unit = await applicationDbContext.Units
            .FirstOrDefaultAsync(x => x.Id == unitToUpdateDto.Id, cancellationToken);

        if (unit == null)
            throw new EntityNotFoundException(typeof(Unit), unitToUpdateDto.Id);

        unit.Name = unitToUpdateDto.Name;
        unit.Description = unitToUpdateDto.Description;
        unit.Code = unitToUpdateDto.Code;

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return unit;
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var unit = await applicationDbContext.Units
                       .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                   ?? throw new EntityNotFoundException(typeof(Unit), id);

        applicationDbContext.Units.Remove(unit);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}