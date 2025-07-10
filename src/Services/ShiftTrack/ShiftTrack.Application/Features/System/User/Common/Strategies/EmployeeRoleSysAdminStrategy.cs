using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Employees.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.Dtos;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;
using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Entities;
using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Enums;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.System.User.Common.Strategies;

public sealed class EmployeeRoleSysAdminStrategy(
    IUnitService unitService,
    IRoleService roleService,
    IEmployeeService employeeService,
    IDepartmentService departmentService,
    IApplicationDbContext applicationDbContext) : IEmployeeRoleStrategy
{
    #region EmployeeRole

    public async Task<EmployeeRole> CreateEmployeeRole(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken)
    {
        var role = await roleService.GetById(dto.RoleId, cancellationToken);

        var employee = await employeeService.GetById(dto.EmployeeId, cancellationToken);

        var employeeRole = new EmployeeRole()
        {
            RoleId = role.Id,
            EmployeeId = employee.Id,
            Scope = RoleScope.Global
        };

        applicationDbContext.EmployeeRoles.Add(employeeRole);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        var departments = new List<Department>();

        if (dto.DepartmentIds is not null
            && dto.DepartmentIds.Any())
        {
            departments = (await departmentService.GetDepartmentsByIds(dto.DepartmentIds, cancellationToken))
                .ToList();
        }

        EmployeeRoleUnit employeeRoleUnit = null;

        if (dto.UnitId is not null)
        {
            employeeRoleUnit = await CreateEmployeeRoleUnit(
                new EmployeeRoleUnitToCreateDto(
                    employeeRole.Id,
                    dto.UnitId.Value,
                    departments.Any() ? RoleScope.Local : RoleScope.Global),
                cancellationToken);

            employeeRole.Scope = RoleScope.Local;
        }

        if (departments.Any()
            && employeeRoleUnit is not null)
        {
            await CreateEmployeeRoleUnitDepartments(
                new EmployeeRoleUnitDepartmentsToCreateDto(
                    employeeRoleUnit.Id,
                    departments.Select(x => x.Id)),
                cancellationToken);
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return employeeRole;
    }

    public async Task DeleteEmployeeRole(long employeeRoleId, CancellationToken cancellationToken)
    {
        var employeeRole = await applicationDbContext.EmployeeRoles
                               .FirstOrDefaultAsync(x => x.Id == employeeRoleId, cancellationToken)
                           ?? throw new EntityNotFoundException(typeof(EmployeeRole), employeeRoleId);

        applicationDbContext.EmployeeRoles.Remove(employeeRole);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region EmployeeRoleUnit

    public async Task<EmployeeRoleUnit> CreateEmployeeRoleUnit(
        EmployeeRoleUnitToCreateDto dto,
        CancellationToken cancellationToken)
    {
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
                                   .FirstOrDefaultAsync(x => x.Id == employeeUnitId, cancellationToken)
                               ?? throw new EntityNotFoundException(typeof(EmployeeRoleUnit), employeeUnitId);

        applicationDbContext.EmployeeRoleUnits.Remove(employeeRoleUnit);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region EmployeeRoleUnitDepartment

    public async Task<IEnumerable<EmployeeRoleUnitDepartment>> CreateEmployeeRoleUnitDepartments(
        EmployeeRoleUnitDepartmentsToCreateDto dto,
        CancellationToken cancellationToken)
    {
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
                                             .FirstOrDefaultAsync(x => x.Id == employeeRoleUnitDepartmentId, cancellationToken)
                                         ?? throw new EntityNotFoundException(typeof(EmployeeRoleUnitDepartment),                                              employeeRoleUnitDepartmentId);

        applicationDbContext.EmployeeRoleUnitDepartments.Remove(employeeRoleUnitDepartment);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion
}