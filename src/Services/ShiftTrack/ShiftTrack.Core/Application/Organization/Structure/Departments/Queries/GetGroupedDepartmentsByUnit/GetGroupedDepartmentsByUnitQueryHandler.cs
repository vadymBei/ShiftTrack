using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Domain.Organization.Structure.Models;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetGroupedDepartmentsByUnit;

public class GetGroupedDepartmentsByUnitQueryHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<GetGroupedDepartmentsByUnitQuery, IEnumerable<GroupedDepartmentsByUnitVM>>
{
    public async Task<IEnumerable<GroupedDepartmentsByUnitVM>> Handle(GetGroupedDepartmentsByUnitQuery request, CancellationToken cancellationToken)
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

        return mapper.Map<List<GroupedDepartmentsByUnitVM>>(groupedDepartments);
    }
}