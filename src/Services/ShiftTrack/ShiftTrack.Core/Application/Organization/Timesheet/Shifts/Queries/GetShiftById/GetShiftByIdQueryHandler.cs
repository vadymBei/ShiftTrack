using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Queries.GetShiftById
{
    public class GetShiftByIdQueryHandler : IRequestHandler<GetShiftByIdQuery, ShiftVM>
    {
        private readonly IMapper _mapper;
        private readonly IShiftService _shiftService;

        public GetShiftByIdQueryHandler(
            IMapper mapper,
            IShiftService shiftService)
        {
            _mapper = mapper;
            _shiftService = shiftService;
        }

        public async Task<ShiftVM> Handle(GetShiftByIdQuery request, CancellationToken cancellationToken)
        {
            var shift = await _shiftService.GetById(request.Id, cancellationToken);

            return _mapper.Map<ShiftVM>(shift);
        }
    }
}
