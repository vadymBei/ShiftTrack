using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Models;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Constants;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.Services;

public class DepartmentService(
    IUnitRepository unitRepository,
    ICurrentUserService currentUserService,
    IEmployeeRoleChecker employeeRoleChecker,
    IDepartmentRepository departmentRepository)
    : IDepartmentService
{
    public async Task<IEnumerable<GroupedDepartmentsByUnit>> GetGroupedDepartmentsByUnit(
        CancellationToken cancellationToken)
    {
        var departments = await departmentRepository.GetAll(cancellationToken);

        var units = await unitRepository.GetAll(cancellationToken);

        var groupedDepartments = new List<GroupedDepartmentsByUnit>();

        foreach (var unit in units)
        {
            var unitDepartments = departments
                .Where(x => x.UnitId == unit.Id);

            groupedDepartments.Add(new GroupedDepartmentsByUnit()
            {
                Unit = unit,
                Departments = unitDepartments.OrderBy(x => x.Name).ToList()
            });
        }

        groupedDepartments = groupedDepartments
            .OrderBy(x => x.Unit.Name)
            .ToList();

        return groupedDepartments;
    }

    public async Task<IEnumerable<Department>> GetDepartmentsByRoles(long unitId, CancellationToken cancellationToken)
    {
        var currentUserEmployeeRoles = currentUserService.Employee.EmployeeRoles;

        var departments = new List<Department>();

        if (employeeRoleChecker.HasCurrentUserSysAdminRole())
        {
            departments = (await departmentRepository.GetAll(cancellationToken)).ToList();
        }
        else if (employeeRoleChecker.HasCurrentUserUnitDirectorRole())
        {
            var employeeRole = currentUserEmployeeRoles
                .FirstOrDefault(x => x.Role.Name == DefaultRolesCatalog.UNIT_DIRECTOR);

            var employeeRoleUnit = employeeRole?.Units
                .FirstOrDefault(x => x.UnitId == unitId);

            if (employeeRoleUnit is not null)
            {
                departments = (await departmentRepository
                        .GetDepartmentsByUnitId((long)employeeRoleUnit.UnitId!, cancellationToken))
                    .ToList();
            }
        }
        else if (employeeRoleChecker.HasCurrentUserDepartmentDirectorRole())
        {
            var employeeRole = currentUserEmployeeRoles
                .FirstOrDefault(x => x.Role.Name == DefaultRolesCatalog.DEPARTMENT_DIRECTOR);

            var employeeRoleUnit = employeeRole?.Units
                .FirstOrDefault(x => x.UnitId == unitId);

            if (employeeRoleUnit != null)
            {
                var departmentIds = employeeRoleUnit.Departments
                    .Select(x => (long)x.DepartmentId!)
                    .Distinct()
                    .ToList();

                departments = (await departmentRepository
                        .GetByIds(departmentIds, cancellationToken))
                    .ToList();
            }
        }
        else if (currentUserService.Employee.Department is not null)
        {
            if (currentUserService.Employee.Department.UnitId != unitId)
                return departments;

            var department = await departmentRepository.GetById(
                (long)currentUserService.Employee.DepartmentId!,
                cancellationToken);

            departments = [department];
        }

        return departments;
    }
}