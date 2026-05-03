using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Queries.GetShifts;

public class GetShiftsQueryHandler(
    IMapper mapper,
    IShiftRepository shiftRepository)
    : IRequestHandler<GetShiftsQuery, IEnumerable<ShiftVm>>
{
    public async Task<IEnumerable<ShiftVm>> Handle(GetShiftsQuery request, CancellationToken cancellationToken)
    {
        var shifts = await shiftRepository.GetAll(cancellationToken);

        return mapper.Map<List<ShiftVm>>(shifts);
    }
}