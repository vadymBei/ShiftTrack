using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Departments.Queries.GetDepartmentsByUnitId;

public class GetDepartmentsByUnitIdQueryHandler(
    IMapper mapper,
    IUnitService unitService,
    IApplicationDbContext dbContext)
    : IRequestHandler<GetDepartmentsByUnitIdQuery, IEnumerable<DepartmentVm>>
{
    public async Task<IEnumerable<DepartmentVm>> Handle(GetDepartmentsByUnitIdQuery request, CancellationToken cancellationToken)
    {
        await unitService
            .GetById(request.UnitId, cancellationToken);

        var departments = await dbContext.Departments
            .Include(x => x.Unit)
            .Where(x => x.UnitId == request.UnitId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return mapper.Map<IEnumerable<DepartmentVm>>(departments);
    }
}