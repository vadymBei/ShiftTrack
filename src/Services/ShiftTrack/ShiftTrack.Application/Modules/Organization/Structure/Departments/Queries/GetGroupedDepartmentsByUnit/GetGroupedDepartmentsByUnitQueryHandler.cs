using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Common.Models;
using ShiftTrack.Application.Modules.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.Queries.GetGroupedDepartmentsByUnit;

public class GetGroupedDepartmentsByUnitQueryHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<GetGroupedDepartmentsByUnitQuery, IEnumerable<GroupedDepartmentsByUnitVm>>
{
    public async Task<IEnumerable<GroupedDepartmentsByUnitVm>> Handle(GetGroupedDepartmentsByUnitQuery request,
        CancellationToken cancellationToken)
    {
        var departments = await applicationDbContext.Departments
            .AsNoTracking()
            .Include(x => x.Unit)
            .ToListAsync(cancellationToken);

        var units = await applicationDbContext.Units
            .AsNoTracking()
            .ToListAsync(cancellationToken);

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

        return mapper.Map<List<GroupedDepartmentsByUnitVm>>(groupedDepartments);
    }
}