using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Domain.Organization.Structure.Models;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetGroupedDepartmentsByUnit
{
    internal class GetGroupedDepartmentsByUnitQueryHandler : IRequestHandler<GetGroupedDepartmentsByUnitQuery, IEnumerable<GroupedDepartmentsByUnitVM>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _applicationDbContext;

        public GetGroupedDepartmentsByUnitQueryHandler(
            IMapper mapper,
            IApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<GroupedDepartmentsByUnitVM>> Handle(GetGroupedDepartmentsByUnitQuery request, CancellationToken cancellationToken)
        {
            var departments = await _applicationDbContext.Departments
                .AsNoTracking()
                .Include(x => x.Unit)
                .ToListAsync(cancellationToken);

            var groupedDepartments = departments
                .GroupBy(x => x.UnitId, (key, values) =>
                    new GroupedDepartmentsByUnit
                    {
                        Unit = values.FirstOrDefault().Unit,
                        Departments = values.OrderBy(x => x.Name).ToList()
                    })
                .OrderBy(x => x.Unit.Name)
                .ToList();                

            return _mapper.Map<List<GroupedDepartmentsByUnitVM>>(groupedDepartments);
        }
    }
}
