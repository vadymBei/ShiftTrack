using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetDepartmentsByUnitId
{
    public class GetDepartmentsByUnitIdQueryHandler : IRequestHandler<GetDepartmentsByUnitIdQuery, IEnumerable<DepartmentVM>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitService _unitService;
        private readonly IApplicationDbContext _dbContext;

        public GetDepartmentsByUnitIdQueryHandler(
            IMapper mapper,
            IUnitService unitService,
            IApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _unitService = unitService;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DepartmentVM>> Handle(GetDepartmentsByUnitIdQuery request, CancellationToken cancellationToken)
        {
            await _unitService
                .GetById(request.UnitId, cancellationToken);

            var departments = await _dbContext.Departments
                .Include(x => x.Unit)
                .Where(x => x.UnitId == request.UnitId)
                .ToArrayAsync(cancellationToken);

            return _mapper.Map<IEnumerable<DepartmentVM>>(departments);
        }
    }
}
