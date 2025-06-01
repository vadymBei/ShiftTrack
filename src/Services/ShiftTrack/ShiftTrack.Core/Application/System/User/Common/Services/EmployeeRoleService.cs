using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Constants;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Exceptions;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Strategies;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Enums;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.System.User.Common.Services;

public class EmployeeRoleService(
    IApplicationDbContext applicationDbContext,
    IEnumerable<IEmployeeRoleStrategy> strategies,
    IEmployeeRoleStrategyFactory employeeRoleStrategyFactory) : IEmployeeRoleService
{
    private IEmployeeRoleStrategy EmployeeRoleStrategy
        => employeeRoleStrategyFactory.GetStrategy();

    #region EmployeeRole

    public async Task<EmployeeRole> CreateEmployeeRole(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken)
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

        return await EmployeeRoleStrategy.CreateEmployeeRole(dto, cancellationToken);
    }

    public Task DeleteEmployeeRole(long employeeRoleId, CancellationToken cancellationToken)
    {
        return EmployeeRoleStrategy.DeleteEmployeeRole(employeeRoleId, cancellationToken);
    }

    public async Task<EmployeeRole> GetEmployeeRoleById(long employeeRoleId, CancellationToken cancellationToken)
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
            .FirstOrDefaultAsync(x => x.Id == employeeRoleId, cancellationToken);

        if (employeeRole is null)
        {
            throw new EntityNotFoundException(typeof(EmployeeRole), employeeRoleId);
        }

        return employeeRole;
    }

    public async Task<IEnumerable<EmployeeRole>> GetEmployeeRolesByEmployeeId(
        long employeeId,
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

    private async Task RecalculateEmployeeRoleScope(long employeeRoleId, CancellationToken cancellationToken)
    {
        var employeeRole = await applicationDbContext.EmployeeRoles
            .Include(x => x.Units)
            .FirstOrDefaultAsync(x => x.Id == employeeRoleId, cancellationToken);

        if (employeeRole is null)
        {
            throw new EntityNotFoundException(typeof(EmployeeRole), employeeRoleId);
        }

        employeeRole.Scope = employeeRole.Units.Count > 0 ? RoleScope.Local : RoleScope.Global;

        applicationDbContext.EmployeeRoles.Update(employeeRole);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<EmployeeRole> CreateSysAdminEmployeeRole(EmployeeRoleToCreateDto dto,
        CancellationToken cancellationToken)
    {
        var sysAdminStrategy = strategies.OfType<EmployeeRoleSysAdminStrategy>().First();
        
        return sysAdminStrategy.CreateEmployeeRole(dto, cancellationToken);
    }
    
    #endregion

    #region EmployeeRoleUnit

    public async Task<EmployeeRoleUnit> CreateEmployeeRoleUnit(EmployeeRoleUnitToCreateDto dto,
        CancellationToken cancellationToken)
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

        var employeeRole = await GetEmployeeRoleById(dto.EmployeeRoleId, cancellationToken);

        var employeeRoleUnit = await EmployeeRoleStrategy.CreateEmployeeRoleUnit(dto, cancellationToken);

        await RecalculateEmployeeRoleScope(employeeRole.Id, cancellationToken);

        return employeeRoleUnit;
    }

    public async Task<EmployeeRoleUnit> GetEmployeeRoleUnitById(long employeeRoleUnitId, CancellationToken cancellationToken)
    {
        var employeeRoleUnit = await applicationDbContext.EmployeeRoleUnits
            .AsNoTracking()
            .Include(x => x.Unit)
            .Include(x => x.Departments)
            .ThenInclude(x => x.Department)
            .FirstOrDefaultAsync(x => x.Id == employeeRoleUnitId, cancellationToken);

        if (employeeRoleUnit is null)
        {
            throw new EntityNotFoundException(typeof(EmployeeRoleUnit), employeeRoleUnitId);
        }

        return employeeRoleUnit;
    }

    public async Task<IEnumerable<EmployeeRoleUnit>> GetEmployeeRoleUnitsByEmployeeRoleId(
        long employeeRoleId,
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

    public async Task DeleteEmployeeRoleUnit(long employeeRoleUnitId, CancellationToken cancellationToken)
    {
        var employeeRoleUnit = await GetEmployeeRoleUnitById(employeeRoleUnitId, cancellationToken);

        var employeeRoleId = employeeRoleUnit.EmployeeRoleId;

        await EmployeeRoleStrategy.DeleteEmployeeRoleUnit(employeeRoleUnitId, cancellationToken);

        if (employeeRoleId is not null)
        {
            await RecalculateEmployeeRoleScope((long)employeeRoleId, cancellationToken);
        }
    }

    private async Task RecalculateEmployeeRoleUnitScope(long employeeRoleUnitId, CancellationToken cancellationToken)
    {
        var employeeRoleUnit = await applicationDbContext.EmployeeRoleUnits
            .Include(x => x.Departments)
            .FirstOrDefaultAsync(x => x.Id == employeeRoleUnitId, cancellationToken);

        employeeRoleUnit.Scope = employeeRoleUnit.Departments.Count > 0 ? RoleScope.Local : RoleScope.Global;

        applicationDbContext.EmployeeRoleUnits.Update(employeeRoleUnit);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region EmployeeRoleUnitDepartment

    public async Task<EmployeeRoleUnitDepartment> CreateEmployeeRoleUnitDepartments(
        EmployeeRoleUnitDepartmentsToCreateDto dto,
        CancellationToken cancellationToken)
    {
        var isAlreadyEmployeeRoleUnitDepartmentExists = await applicationDbContext.EmployeeRoleUnitDepartments
            .AnyAsync(x => x.EmployeeRoleUnitId == dto.EmployeeRoleUnitId
                           && dto.DepartmentIds.Contains((long)x.DepartmentId),
                cancellationToken);

        if (isAlreadyEmployeeRoleUnitDepartmentExists)
        {
            throw new EmployeeRoleAlreadyExistException(
                UserExceptionsLocalization.EMPLOYEE_ROLE_UNIT_DEPARTMENT_ALREADY_EXISTS,
                nameof(UserExceptionsLocalization.EMPLOYEE_ROLE_UNIT_DEPARTMENT_ALREADY_EXISTS));
        }

        var employeeRoleUnitDepartments = await EmployeeRoleStrategy
            .CreateEmployeeRoleUnitDepartments(dto, cancellationToken);

        await RecalculateEmployeeRoleUnitScope(dto.EmployeeRoleUnitId, cancellationToken);

        return employeeRoleUnitDepartments.FirstOrDefault();
    }

    public async Task<IEnumerable<EmployeeRoleUnitDepartment>> GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitId(
        long employeeRoleUnitId,
        CancellationToken cancellationToken)
    {
        var employeeRoleUnitDepartments = await applicationDbContext.EmployeeRoleUnitDepartments
            .AsNoTracking()
            .Include(x => x.Department)
            .Where(x => x.EmployeeRoleUnitId == employeeRoleUnitId)
            .ToListAsync(cancellationToken);

        return employeeRoleUnitDepartments;
    }

    public async Task DeleteEmployeeRoleUnitDepartment(
        long employeeRoleUnitDepartmentId,
        CancellationToken cancellationToken)
    {
        var employeeRoleUnitDepartment = await applicationDbContext.EmployeeRoleUnitDepartments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == employeeRoleUnitDepartmentId, cancellationToken);

        var employeeRoleUnitId = employeeRoleUnitDepartment.EmployeeRoleUnitId;

        await EmployeeRoleStrategy.DeleteEmployeeRoleUnitDepartment(employeeRoleUnitDepartmentId, cancellationToken);

        if (employeeRoleUnitId is not null)
        {
            await RecalculateEmployeeRoleUnitScope((long)employeeRoleUnitId, cancellationToken);
        }
    }

    #endregion
}