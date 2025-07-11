using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.Models;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Departments.Queries.GetGroupedDepartmentsByUnit;

public class GetGroupedDepartmentsByUnitQueryHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<GetGroupedDepartmentsByUnitQuery, IEnumerable<GroupedDepartmentsByUnitVm>>
{
    public async Task<IEnumerable<GroupedDepartmentsByUnitVm>> Handle(GetGroupedDepartmentsByUnitQuery request, CancellationToken cancellationToken)
    {
        var departments = await applicationDbContext.Departments
            .AsNoTracking()
            .Include(x => x.Unit)
            .ToListAsync(cancellationToken);

        var groupedDepartments = departments
            .Where(x => x.UnitId is not null)
            .GroupBy(x => x.UnitId, (key, values) =>
                new GroupedDepartmentsByUnit
                {
                    Unit = values.FirstOrDefault().Unit,
                    Departments = values
                        .OrderBy(x => x.Name)
                        .ToList()
                })
            .OrderBy(x => x.Unit.Name)
            .ToList();

        return mapper.Map<List<GroupedDepartmentsByUnitVm>>(groupedDepartments);
    }
}