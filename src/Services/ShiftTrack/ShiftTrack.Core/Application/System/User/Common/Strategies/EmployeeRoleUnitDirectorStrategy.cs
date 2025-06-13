using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Constants;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Exceptions;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Enums;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.System.User.Common.Strategies;

public sealed class EmployeeRoleUnitDirectorStrategy(
    IRoleService roleService,
    IUnitService unitService,
    IEmployeeService employeeService,
    IDepartmentService departmentService,
    ICurrentUserService currentUserService,
    IApplicationDbContext applicationDbContext) : IEmployeeRoleStrategy
{
    #region EmployeeRole

    public async Task<EmployeeRole> CreateEmployeeRole(
        EmployeeRoleToCreateDto dto,
        CancellationToken cancellationToken)
    {
        if (dto.UnitId is null
            || !dto.DepartmentIds.Any())
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.CANNOT_ASSIGN_GLOBAL_SCOPE,
                nameof(UserExceptionsLocalization.CANNOT_ASSIGN_GLOBAL_SCOPE));
        }

        var employee = await employeeService.GetById(dto.EmployeeId, cancellationToken);

        var currentUser = currentUserService.Employee;

        if (employee.Department is null
            || currentUser.Department.UnitId != dto.UnitId
            || !currentUser.EmployeeRoles.Any(x => x.Role.Name == DefaultRolesCatalog.UNIT_DIRECTOR
                                                   && x.Units.Any(u => u.UnitId == employee.Department.UnitId)))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.CREATE_ROLE_UNIT_OUT_OF_SCOPE,
                nameof(UserExceptionsLocalization.CREATE_ROLE_UNIT_OUT_OF_SCOPE));
        }

        var role = await roleService.GetById(dto.RoleId, cancellationToken);

        if (role.Name is not (DefaultRolesCatalog.DEPARTMENT_ADMIN
            or DefaultRolesCatalog.DEPARTMENT_DIRECTOR
            or DefaultRolesCatalog.DEPARTMENT_STYLIST))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.UNIT_DIRECTOR_INVALID_ROLE,
                nameof(UserExceptionsLocalization.UNIT_DIRECTOR_INVALID_ROLE));
        }

        var employeeRole = new EmployeeRole()
        {
            RoleId = role.Id,
            EmployeeId = employee.Id,
            Scope = RoleScope.Global
        };

        applicationDbContext.EmployeeRoles.Add(employeeRole);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        var departments = await departmentService
            .GetDepartmentsByIds(dto.DepartmentIds, cancellationToken);

        var employeeRoleUnit = await CreateEmployeeRoleUnit(
            new EmployeeRoleUnitToCreateDto(
                employeeRole.Id,
                dto.UnitId.Value,
                RoleScope.Local),
            cancellationToken);

        await CreateEmployeeRoleUnitDepartments(
            new EmployeeRoleUnitDepartmentsToCreateDto(
                employeeRoleUnit.Id,
                departments.Select(x => x.Id)),
            cancellationToken);

        return employeeRole;
    }

    public async Task DeleteEmployeeRole(long employeeRoleId, CancellationToken cancellationToken)
    {
        var employeeRole = await applicationDbContext.EmployeeRoles
                               .Include(x => x.Employee)
                               .ThenInclude(x => x.Department)
                               .ThenInclude(x => x.Unit)
                               .FirstOrDefaultAsync(x => x.Id == employeeRoleId, cancellationToken)
                           ?? throw new EntityNotFoundException(typeof(EmployeeRole), employeeRoleId);

        if (employeeRole.Employee.Department is null
            || employeeRole.Employee.Department.UnitId != currentUserService.Employee.Department.UnitId)
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT,
                nameof(UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT));
        }

        applicationDbContext.EmployeeRoles.Remove(employeeRole);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region EmployeeRoleUnit

    public async Task<EmployeeRoleUnit> CreateEmployeeRoleUnit(EmployeeRoleUnitToCreateDto dto,
        CancellationToken cancellationToken)
    {
        var employeeRole = await applicationDbContext.EmployeeRoles
                               .AsNoTracking()
                               .Include(x => x.Employee)
                               .ThenInclude(x => x.Department)
                               .FirstOrDefaultAsync(x => x.Id == dto.EmployeeRoleId, cancellationToken)
                           ?? throw new EntityNotFoundException(typeof(EmployeeRole), dto.EmployeeRoleId);

        if (employeeRole.Employee.Department is null
            || !currentUserService.Employee.EmployeeRoles
                .Any(x => x.Role.Name == DefaultRolesCatalog.UNIT_DIRECTOR
                          && x.Units.Any(u =>
                              u.UnitId == employeeRole.Employee.Department
                                  .UnitId)))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.CREATE_ROLE_UNIT_OUT_OF_SCOPE,
                nameof(UserExceptionsLocalization.CREATE_ROLE_UNIT_OUT_OF_SCOPE));
        }

        var unit = await unitService.GetById(dto.UnitId, cancellationToken);

        var employeeRoleUnit = new EmployeeRoleUnit()
        {
            EmployeeRoleId = dto.EmployeeRoleId,
            UnitId = unit.Id,
            Scope = dto.Scope
        };

        applicationDbContext.EmployeeRoleUnits.Add(employeeRoleUnit);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return employeeRoleUnit;
    }

    public async Task DeleteEmployeeRoleUnit(long employeeUnitId, CancellationToken cancellationToken)
    {
        var employeeRoleUnit = await applicationDbContext.EmployeeRoleUnits
                                   .Include(x => x.EmployeeRole)
                                   .ThenInclude(x => x.Employee)
                                   .ThenInclude(x => x.Department)
                                   .FirstOrDefaultAsync(x => x.Id == employeeUnitId, cancellationToken)
                               ?? throw new EntityNotFoundException(typeof(EmployeeRoleUnit), employeeUnitId);

        if (employeeRoleUnit.EmployeeRole.Employee.Department is null
            || !currentUserService.Employee.EmployeeRoles
                .Any(x => x.Role.Name == DefaultRolesCatalog.UNIT_DIRECTOR
                          && x.Units.Any(u =>
                              u.UnitId == employeeRoleUnit.EmployeeRole.Employee.Department.UnitId)))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT,
                nameof(UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT));
        }

        applicationDbContext.EmployeeRoleUnits.Remove(employeeRoleUnit);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region EmployeeRoleUnitDepartment

    public async Task<IEnumerable<EmployeeRoleUnitDepartment>> CreateEmployeeRoleUnitDepartments(
        EmployeeRoleUnitDepartmentsToCreateDto dto,
        CancellationToken cancellationToken)
    {
        var employeeRoleUnit = await applicationDbContext.EmployeeRoleUnits
                                   .AsNoTracking()
                                   .Include(x => x.Unit)
                                   .Include(x => x.Departments)
                                   .FirstOrDefaultAsync(x => x.Id == dto.EmployeeRoleUnitId, cancellationToken)
                               ?? throw new EntityNotFoundException(typeof(EmployeeRoleUnit), dto.EmployeeRoleUnitId);

        if (!currentUserService.Employee.EmployeeRoles
                .Any(x => x.Role.Name == DefaultRolesCatalog.UNIT_DIRECTOR
                          && x.Units.Any(u =>
                              u.UnitId == employeeRoleUnit.UnitId)))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.CREATE_ROLE_UNIT_OUT_OF_SCOPE,
                nameof(UserExceptionsLocalization.CREATE_ROLE_UNIT_OUT_OF_SCOPE));
        }

        var employeeRoleUnitDepartments = dto.DepartmentIds
            .Select(x => new EmployeeRoleUnitDepartment()
            {
                EmployeeRoleUnitId = dto.EmployeeRoleUnitId,
                DepartmentId = x
            })
            .ToList();

        applicationDbContext.EmployeeRoleUnitDepartments.AddRange(employeeRoleUnitDepartments);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return employeeRoleUnitDepartments;
    }

    public async Task DeleteEmployeeRoleUnitDepartment(long employeeRoleUnitDepartmentId,
        CancellationToken cancellationToken)
    {
        var employeeRoleUnitDepartment = await applicationDbContext.EmployeeRoleUnitDepartments
                                             .Include(x => x.EmployeeRoleUnit)
                                             .ThenInclude(x => x.EmployeeRole)
                                             .ThenInclude(x => x.Employee)
                                             .ThenInclude(x => x.Department)
                                             .FirstOrDefaultAsync(x => x.Id == employeeRoleUnitDepartmentId, cancellationToken)
                                         ?? throw new EntityNotFoundException(typeof(EmployeeRoleUnitDepartment), employeeRoleUnitDepartmentId);

        var employeeRole = employeeRoleUnitDepartment.EmployeeRoleUnit.EmployeeRole;

        if (!currentUserService.Employee.EmployeeRoles
                .Any(x => x.Role.Name == DefaultRolesCatalog.UNIT_DIRECTOR
                          && x.Units.Any(u =>
                              u.UnitId == employeeRole.Employee.Department.UnitId)))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT,
                nameof(UserExceptionsLocalization.DELETE_ROLE_WRONG_UNIT));
        }

        applicationDbContext.EmployeeRoleUnitDepartments.Remove(employeeRoleUnitDepartment);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion
}