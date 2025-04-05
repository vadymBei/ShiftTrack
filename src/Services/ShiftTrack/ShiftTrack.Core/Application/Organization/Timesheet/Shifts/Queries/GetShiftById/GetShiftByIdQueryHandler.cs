using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Queries.GetShiftById;

public class GetShiftByIdQueryHandler(
    IMapper mapper,
    IShiftService shiftService) : IRequestHandler<GetShiftByIdQuery, ShiftVM>
{
    public async Task<ShiftVM> Handle(GetShiftByIdQuery request, CancellationToken cancellationToken)
    {
        var shift = await shiftService.GetById(request.Id, cancellationToken);

        return mapper.Map<ShiftVM>(shift);
    }
}