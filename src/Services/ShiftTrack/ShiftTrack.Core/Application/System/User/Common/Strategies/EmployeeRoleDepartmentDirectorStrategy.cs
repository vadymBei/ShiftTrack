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

public sealed class EmployeeRoleDepartmentDirectorStrategy(
    IRoleService roleService,
    IUnitService unitService,
    IEmployeeService employeeService,
    IDepartmentService departmentService,
    ICurrentUserService currentUserService,
    IApplicationDbContext applicationDbContext) : IEmployeeRoleStrategy
{
    #region EmployeeRole

    public async Task<EmployeeRole> CreateEmployeeRole(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken)
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
            || !dto.DepartmentIds.All(x => x == currentUser.DepartmentId)
            || !currentUser.EmployeeRoles.Any(x => x.Role.Name == DefaultRolesCatalog.DEPARTMENT_DIRECTOR
                                                   && x.Units.Any(u => u.Departments.Any(d =>
                                                       dto.DepartmentIds.Contains((long)d.DepartmentId)))
                                                   && x.Units.Any(u => u.UnitId == employee.Department.UnitId))
            )
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.CREATE_ROLE_UNIT_DEPARTMENT_OUT_OF_SCOPE,
                nameof(UserExceptionsLocalization.CREATE_ROLE_UNIT_DEPARTMENT_OUT_OF_SCOPE));
        }

        var role = await roleService.GetById(dto.RoleId, cancellationToken);

        if (role.Name is not (DefaultRolesCatalog.DEPARTMENT_ADMIN
            or DefaultRolesCatalog.DEPARTMENT_STYLIST))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE,
                nameof(UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE));
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

        var role = await roleService.GetById(employeeRole.RoleId, cancellationToken);

        if (role.Name is not (DefaultRolesCatalog.DEPARTMENT_ADMIN
            or DefaultRolesCatalog.DEPARTMENT_STYLIST))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE,
                nameof(UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE));
        }

        if (employeeRole.Employee.Department is null
            || employeeRole.Employee.Department.Id != currentUserService.Employee.DepartmentId
            || employeeRole.Employee.Department.UnitId != currentUserService.Employee.Department.UnitId)
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DELETE_ROLE_WRONG_DEPARTMENT,
                nameof(UserExceptionsLocalization.DELETE_ROLE_WRONG_DEPARTMENT));
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
            .Include(x => x.Role)
            .Include(x => x.Employee)
            .ThenInclude(x => x.Department)
            .FirstOrDefaultAsync(x => x.Id == dto.EmployeeRoleId, cancellationToken);

        if (employeeRole.Role.Name is not (DefaultRolesCatalog.DEPARTMENT_ADMIN
            or DefaultRolesCatalog.DEPARTMENT_STYLIST))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE,
                nameof(UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE));
        }

        if (employeeRole.Employee.Department is null
            || !currentUserService.Employee.EmployeeRoles
                .Any(x => x.Role.Name == DefaultRolesCatalog.DEPARTMENT_DIRECTOR
                          && x.Units.Any(u =>
                              u.UnitId == employeeRole.Employee.Department.UnitId
                              && u.Departments.Any(d => d.DepartmentId == employeeRole.Employee.DepartmentId))))
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
                                   .ThenInclude(x => x.Role)
                                   .Include(x => x.EmployeeRole)
                                   .ThenInclude(x => x.Employee)
                                   .ThenInclude(x => x.Department)
                                   .FirstOrDefaultAsync(x => x.Id == employeeUnitId, cancellationToken)
                               ?? throw new EntityNotFoundException(typeof(EmployeeRoleUnit), employeeUnitId);

        if (employeeRoleUnit.EmployeeRole.Role.Name is not (DefaultRolesCatalog.DEPARTMENT_ADMIN
            or DefaultRolesCatalog.DEPARTMENT_STYLIST))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE,
                nameof(UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE));
        }

        if (employeeRoleUnit.EmployeeRole.Employee.Department is null
            || !currentUserService.Employee.EmployeeRoles
                .Any(x => x.Role.Name == DefaultRolesCatalog.DEPARTMENT_DIRECTOR
                          && x.Units.Any(u =>
                              u.UnitId == employeeRoleUnit.EmployeeRole.Employee.Department.UnitId
                              && u.Departments.Any(d =>
                                  d.DepartmentId == employeeRoleUnit.EmployeeRole.Employee.DepartmentId))))
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
        EmployeeRoleUnitDepartmentsToCreateDto dto, CancellationToken cancellationToken)
    {
        var employeeRoleUnit = await applicationDbContext.EmployeeRoleUnits
                                   .AsNoTracking()
                                   .Include(x => x.EmployeeRole)
                                   .ThenInclude(x => x.Role)
                                   .Include(x => x.EmployeeRole)
                                   .ThenInclude(x => x.Employee)
                                   .Include(x => x.Unit)
                                   .Include(x => x.Departments)
                                   .FirstOrDefaultAsync(x => x.Id == dto.EmployeeRoleUnitId, cancellationToken)
                               ?? throw new EntityNotFoundException(typeof(EmployeeRoleUnit), dto.EmployeeRoleUnitId);

        if (employeeRoleUnit.EmployeeRole.Role.Name is not (DefaultRolesCatalog.DEPARTMENT_ADMIN
            or DefaultRolesCatalog.DEPARTMENT_STYLIST))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE,
                nameof(UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE));
        }
        
        if (!currentUserService.Employee.EmployeeRoles
                .Any(x => x.Role.Name == DefaultRolesCatalog.DEPARTMENT_DIRECTOR
                          && x.Units.Any(u =>
                              u.UnitId == employeeRoleUnit.UnitId
                              && u.Departments.Any(d =>
                                  d.DepartmentId == employeeRoleUnit.EmployeeRole.Employee.DepartmentId))))
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

    public async Task DeleteEmployeeRoleUnitDepartment(long employeeRoleUnitDepartmentId, CancellationToken cancellationToken)
    {
        var employeeRoleUnitDepartment = await applicationDbContext.EmployeeRoleUnitDepartments
                                             .Include(x => x.EmployeeRoleUnit)
                                             .ThenInclude(x => x.EmployeeRole)
                                             .ThenInclude(x => x.Employee)
                                             .ThenInclude(x => x.Department)
                                             .Include(x => x.EmployeeRoleUnit)
                                             .ThenInclude(x => x.EmployeeRole)
                                             .ThenInclude(x => x.Role)
                                             .FirstOrDefaultAsync(x => x.Id == employeeRoleUnitDepartmentId, cancellationToken)
                                         ?? throw new EntityNotFoundException(typeof(EmployeeRoleUnitDepartment), employeeRoleUnitDepartmentId);

        if (employeeRoleUnitDepartment.EmployeeRoleUnit.EmployeeRole.Role.Name is not (DefaultRolesCatalog.DEPARTMENT_ADMIN
            or DefaultRolesCatalog.DEPARTMENT_STYLIST))
        {
            throw new EmployeeRoleException(
                UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE,
                nameof(UserExceptionsLocalization.DEPARTMENT_DIRECTOR_INVALID_ROLE));
        }
        
        var employeeRole = employeeRoleUnitDepartment.EmployeeRoleUnit.EmployeeRole;

        if (!currentUserService.Employee.EmployeeRoles
                .Any(x => x.Role.Name == DefaultRolesCatalog.DEPARTMENT_DIRECTOR
                          && x.Units.Any(u =>
                              u.UnitId == employeeRole.Employee.Department.UnitId
                              && u.Departments.Any(d => d.DepartmentId == employeeRole.Employee.DepartmentId))))
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