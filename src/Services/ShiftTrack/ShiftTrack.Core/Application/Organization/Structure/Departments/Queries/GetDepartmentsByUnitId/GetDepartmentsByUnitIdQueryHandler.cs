using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetDepartmentsByUnitId
{
    public class GetDepartmentsByUnitIdQueryHandler : IRequestHandler<GetDepartmentsByUnitIdQuery, IEnumerable<DepartmentVM>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _dbContext;

        public GetDepartmentsByUnitIdQueryHandler(
            IMapper mapper,
            IApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DepartmentVM>> Handle(GetDepartmentsByUnitIdQuery request, CancellationToken cancellationToken)
        {
            var departments = await _dbContext.Departments
                .Where(x => x.UnitId == request.UnitId)
                .ToArrayAsync(cancellationToken);

            return _mapper.Map<IEnumerable<DepartmentVM>>(departments);
        }
    }
}
