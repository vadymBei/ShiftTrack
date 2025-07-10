using AutoMapper;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Shifts.Queries.GetShiftById;

public class GetShiftByIdQueryHandler(
    IMapper mapper,
    IShiftService shiftService) : IRequestHandler<GetShiftByIdQuery, ShiftVm>
{
    public async Task<ShiftVm> Handle(GetShiftByIdQuery request, CancellationToken cancellationToken)
    {
        var shift = await shiftService.GetById(request.Id, cancellationToken);

        return mapper.Map<ShiftVm>(shift);
    }
}