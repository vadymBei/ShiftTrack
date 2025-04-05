using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetDepartmentsByUnitId;

public class GetDepartmentsByUnitIdQueryHandler(
    IMapper mapper,
    IUnitService unitService,
    IApplicationDbContext dbContext)
    : IRequestHandler<GetDepartmentsByUnitIdQuery, IEnumerable<DepartmentVM>>
{
    public async Task<IEnumerable<DepartmentVM>> Handle(GetDepartmentsByUnitIdQuery request, CancellationToken cancellationToken)
    {
        await unitService
            .GetById(request.UnitId, cancellationToken);

        var departments = await dbContext.Departments
            .Include(x => x.Unit)
            .Where(x => x.UnitId == request.UnitId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return mapper.Map<IEnumerable<DepartmentVM>>(departments);
    }
}