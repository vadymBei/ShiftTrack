using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Application.Features.System.User.Common.Constants;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Departments.Queries.GetDepartmentsByRoles;

public class GetDepartmentsByRolesQueryHandler(
    IMapper mapper,
    ICurrentUserService currentUserService,
    IEmployeeRoleChecker employeeRoleChecker,
    IApplicationDbContext applicationDbContext) : IRequestHandler<GetDepartmentsByRolesQuery, IEnumerable<DepartmentVm>>
{
    public async Task<IEnumerable<DepartmentVm>> Handle(GetDepartmentsByRolesQuery request,
        CancellationToken cancellationToken = default)
    {
        var currentUserEmployeeRoles = currentUserService.Employee.EmployeeRoles;

        var departmentQuery = applicationDbContext.Departments
            .AsNoTracking();

        var departments = new List<Department>();

        if (employeeRoleChecker.HasCurrentUserSysAdminRole())
        {
            departments = await departmentQuery
                .Where(x => x.UnitId == request.UnitId)
                .ToListAsync(cancellationToken);
        }
        else if (employeeRoleChecker.HasCurrentUserUnitDirectorRole())
        {
            var employeeRole = currentUserEmployeeRoles
                .FirstOrDefault(x => x.Role.Name == DefaultRolesCatalog.UNIT_DIRECTOR);

            var employeeRoleUnit = employeeRole.Units
                .FirstOrDefault(x => x.UnitId == request.UnitId);

            if (employeeRoleUnit is not null)
            {
                departments = await departmentQuery
                    .Where(x => x.UnitId == employeeRoleUnit.UnitId)
                    .ToListAsync(cancellationToken);
            }
        }
        else if (employeeRoleChecker.HasCurrentUserDepartmentDirectorRole())
        {
            var employeeRole = currentUserEmployeeRoles
                .FirstOrDefault(x => x.Role.Name == DefaultRolesCatalog.DEPARTMENT_DIRECTOR);

            var employeeRoleUnit = employeeRole?.Units
                .FirstOrDefault(x => x.UnitId == request.UnitId);

            if (employeeRoleUnit != null)
            {
                var departmentIds = employeeRoleUnit.Departments
                    .Select(x => x.DepartmentId)
                    .Distinct()
                    .ToList();

                departments = await departmentQuery
                    .Where(x => departmentIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);
            }
        }
        else if (currentUserService.Employee.Department is not null)
        {
            if (currentUserService.Employee.Department.UnitId == request.UnitId)
            {
                departments = await departmentQuery
                    .Where(x => x.Id == currentUserService.Employee.DepartmentId)
                    .ToListAsync(cancellationToken);
            }
        }

        return mapper.Map<IEnumerable<DepartmentVm>>(departments);
    }
}