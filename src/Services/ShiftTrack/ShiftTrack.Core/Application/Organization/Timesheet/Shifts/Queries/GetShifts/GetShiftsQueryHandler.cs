using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Queries.GetShifts
{
    public class GetShiftsQueryHandler : IRequestHandler<GetShiftsQuery, IEnumerable<ShiftVM>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _applicationDbContext;

        public GetShiftsQueryHandler(
            IMapper mapper,
            IApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<ShiftVM>> Handle(GetShiftsQuery request, CancellationToken cancellationToken)
        {
            var shifts = await _applicationDbContext.Shifts
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<ShiftVM>>(shifts);
        }
    }
}
