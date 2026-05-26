using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.System.User.Common.Constants;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Exceptions;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Enums;
using ShiftTrack.Infrastructure.Common.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Infrastructure.Modules.System.User.EmployeeRoles.Repositories;

public class EmployeeRoleRepository(
    IApplicationDbContext applicationDbContext) : IEmployeeRoleRepository
{
    public async Task<EmployeeRole> Create(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken)
    {
        var employeeRole = new EmployeeRole()
        {
            RoleId = dto.RoleId,
            EmployeeId = dto.EmployeeId,
            Scope = RoleScope.Global
        };

        applicationDbContext.EmployeeRoles.Add(employeeRole);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return employeeRole;
    }

    public async Task CheckIfExists(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken)
    {
        var isEmployeeRoleExists = await applicationDbContext.EmployeeRoles
            .AnyAsync(x => x.RoleId == dto.RoleId
                           && x.EmployeeId == dto.EmployeeId, cancellationToken);

        if (isEmployeeRoleExists)
        {
            throw new EmployeeRoleAlreadyExistException(
                UserExceptionsLocalization.EMPLOYEE_ROLE_ALREADY_EXISTS,
                nameof(UserExceptionsLocalization.EMPLOYEE_ROLE_ALREADY_EXISTS));
        }
    }

    public async Task<EmployeeRole> GetById(long id, CancellationToken cancellationToken)
    {
        var employeeRole = await applicationDbContext.EmployeeRoles
                               .AsNoTracking()
                               .Include(x => x.Employee)
                               .ThenInclude(x => x.Department)
                               .Include(x => x.Role)
                               .Include(x => x.Units)
                               .ThenInclude(x => x.Unit)
                               .Include(x => x.Units)
                               .ThenInclude(x => x.Departments)
                               .ThenInclude(x => x.Department)
                               .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                           ?? throw new EntityNotFoundException(typeof(EmployeeRole), id);

        return employeeRole;
    }

    public async Task<IEnumerable<EmployeeRole>> GetListByEmployeeId(long employeeId,
        CancellationToken cancellationToken)
    {
        return await applicationDbContext.EmployeeRoles
            .AsNoTracking()
            .Include(x => x.Role)
            .Include(x => x.Units)
            .ThenInclude(x => x.Unit)
            .Include(x => x.Units)
            .ThenInclude(x => x.Departments)
            .ThenInclude(x => x.Department)
            .Where(x => x.EmployeeId == employeeId)
            .ToListAsync(cancellationToken);
    }

    public async Task RecalculateScope(long id, CancellationToken cancellationToken)
    {
        var employeeRole = await applicationDbContext.EmployeeRoles
                               .Include(x => x.Units)
                               .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                           ?? throw new EntityNotFoundException(typeof(EmployeeRole), id);

        employeeRole.Scope = employeeRole.Units.Count > 0 ? RoleScope.Local : RoleScope.Global;

        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var employeeRole = await applicationDbContext.EmployeeRoles
                               .Include(x => x.Employee)
                               .ThenInclude(x => x.Department)
                               .ThenInclude(x => x.Unit)
                               .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                           ?? throw new EntityNotFoundException(typeof(EmployeeRole), id);

        applicationDbContext.EmployeeRoles.Remove(employeeRole);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}