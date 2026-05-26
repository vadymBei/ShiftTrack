using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Queries.GetShiftById;

public class GetShiftByIdQueryHandler(
    IMapper mapper,
    IShiftRepository shiftRepository) : IRequestHandler<GetShiftByIdQuery, ShiftVm>
{
    public async Task<ShiftVm> Handle(GetShiftByIdQuery request, CancellationToken cancellationToken)
    {
        var shift = await shiftRepository.GetById(request.Id, cancellationToken);

        return mapper.Map<ShiftVm>(shift);
    }
}