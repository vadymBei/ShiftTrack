using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.System.User.Common.Constants;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Exceptions;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Enums;
using ShiftTrack.Infrastructure.Common.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Infrastructure.Modules.System.User.EmployeeRoleUnits.Repositories;

public class EmployeeRoleUnitRepository(
    IApplicationDbContext applicationDbContext) : IEmployeeRoleUnitRepository
{
    public async Task<EmployeeRoleUnit> Create(EmployeeRoleUnitToCreateDto dto, CancellationToken cancellationToken)
    {
        var employeeRoleUnit = new EmployeeRoleUnit()
        {
            EmployeeRoleId = dto.EmployeeRoleId,
            UnitId = dto.UnitId,
            Scope = dto.Scope
        };

        applicationDbContext.EmployeeRoleUnits.Add(employeeRoleUnit);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return employeeRoleUnit;
    }

    public async Task<EmployeeRoleUnit> GetById(long id, CancellationToken cancellationToken)
    {
        var employeeRoleUnit = await applicationDbContext.EmployeeRoleUnits
                                   .AsNoTracking()
                                   .Include(x => x.EmployeeRole)
                                   .ThenInclude(x => x.Role)
                                   .Include(x => x.EmployeeRole)
                                   .ThenInclude(x => x.Employee)
                                   .Include(x => x.Unit)
                                   .Include(x => x.Departments)
                                   .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                               ?? throw new EntityNotFoundException(typeof(EmployeeRoleUnit), id);

        return employeeRoleUnit;
    }

    public async Task<IEnumerable<EmployeeRoleUnit>> GetListByEmployeeRoleId(long employeeRoleId,
        CancellationToken cancellationToken)
    {
        var employeeRoleUnits = await applicationDbContext.EmployeeRoleUnits
            .AsNoTracking()
            .Include(x => x.Unit)
            .Include(x => x.Departments)
            .ThenInclude(x => x.Department)
            .Where(x => x.EmployeeRoleId == employeeRoleId)
            .ToListAsync(cancellationToken);

        return employeeRoleUnits;
    }

    public async Task<IEnumerable<EmployeeRoleUnit>> GetListByUnitId(long unitId, CancellationToken cancellationToken)
    {
        return await applicationDbContext.EmployeeRoleUnits
            .AsNoTracking()
            .Include(x => x.EmployeeRole)
            .ThenInclude(x => x.Employee)
            .Include(x => x.EmployeeRole)
            .ThenInclude(x => x.Role)
            .Where(x => x.UnitId == unitId)
            .ToListAsync(cancellationToken);
    }

    public async Task CheckIfExists(EmployeeRoleUnitToCreateDto dto, CancellationToken cancellationToken)
    {
        var isAlreadyEmployeeRoleUnitExists = await applicationDbContext.EmployeeRoleUnits
            .AnyAsync(x => x.EmployeeRoleId == dto.EmployeeRoleId
                           && x.UnitId == dto.UnitId, cancellationToken);

        if (isAlreadyEmployeeRoleUnitExists)
        {
            throw new EmployeeRoleAlreadyExistException(
                UserExceptionsLocalization.EMPLOYEE_ROLE_UNIT_ALREADY_EXISTS,
                nameof(UserExceptionsLocalization.EMPLOYEE_ROLE_UNIT_ALREADY_EXISTS));
        }
    }

    public async Task RecalculateScope(long employeeRoleUnitId, CancellationToken cancellationToken)
    {
        var employeeRoleUnit = await applicationDbContext.EmployeeRoleUnits
            .Include(x => x.Departments)
            .FirstOrDefaultAsync(x => x.Id == employeeRoleUnitId, cancellationToken);

        employeeRoleUnit.Scope = employeeRoleUnit.Departments.Count > 0 ? RoleScope.Local : RoleScope.Global;

        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var employeeRoleUnit = await applicationDbContext.EmployeeRoleUnits
                                   .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                               ?? throw new EntityNotFoundException(typeof(EmployeeRoleUnit), id);

        applicationDbContext.EmployeeRoleUnits.Remove(employeeRoleUnit);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}