using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Application.Features.System.User.Common.Constants;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Units.Queries.GetUnitsByRoles;

public class GetUnitsByRolesQueryHandler(
    IMapper mapper,
    ICurrentUserService currentUserService,
    IEmployeeRoleChecker employeeRoleChecker,
    IApplicationDbContext applicationDbContext) : IRequestHandler<GetUnitsByRolesQuery, IEnumerable<UnitVm>>
{
    public async Task<IEnumerable<UnitVm>> Handle(GetUnitsByRolesQuery request,
        CancellationToken cancellationToken = default)
    {
        var currentUserEmployeeRoles = currentUserService.Employee.EmployeeRoles;

        var unitQuery = applicationDbContext.Units
            .AsNoTracking();

        var units = new List<Unit>();

        if (employeeRoleChecker.HasCurrentUserSysAdminRole())
        {
            units = await unitQuery.ToListAsync(cancellationToken);
        }
        else if (employeeRoleChecker.HasCurrentUserUnitDirectorRole())
        {
            var employeeRole = currentUserEmployeeRoles
                .FirstOrDefault(x => x.Role.Name == DefaultRolesCatalog.UNIT_DIRECTOR);

            var unitIds = employeeRole.Units
                .Select(x => x.UnitId)
                .Distinct()
                .ToList();

            units = await unitQuery
                .Where(x => unitIds.Contains(x.Id))
                .ToListAsync(cancellationToken);
        }
        else if (employeeRoleChecker.HasCurrentUserDepartmentDirectorRole())
        {
            var employeeRole = currentUserEmployeeRoles
                .FirstOrDefault(x => x.Role.Name == DefaultRolesCatalog.DEPARTMENT_DIRECTOR);

            var unitId = employeeRole.Units.FirstOrDefault()?.UnitId
                         ?? currentUserService.Employee.Department.UnitId;

            units = await unitQuery
                .Where(x => x.Id == unitId)
                .ToListAsync(cancellationToken);
        }
        else if (currentUserService.Employee.Department is not null)
        {
            units = await unitQuery
                .Where(x => x.Id == currentUserService.Employee.Department.UnitId)
                .ToListAsync(cancellationToken);
        }

        return mapper.Map<IEnumerable<UnitVm>>(units);
    }
}