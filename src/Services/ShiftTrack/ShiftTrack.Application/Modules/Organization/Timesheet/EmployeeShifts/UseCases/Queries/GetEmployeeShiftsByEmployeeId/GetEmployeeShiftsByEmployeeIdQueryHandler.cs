using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.UseCases.Queries.GetEmployeeShiftsByEmployeeId;

public class GetEmployeeShiftsByEmployeeIdQueryHandler(
    IMapper mapper,
    IEmployeeShiftService employeeShiftService) : IRequestHandler<GetEmployeeShiftsByEmployeeIdQuery, IEnumerable<EmployeeShiftVm>>
{
    public async Task<IEnumerable<EmployeeShiftVm>> Handle(GetEmployeeShiftsByEmployeeIdQuery request, CancellationToken cancellationToken = default)
    {
        var employeeShifts = await employeeShiftService.GetEmployeeShiftsByEmployeeId(
            request.Data,
            cancellationToken);
        
        return mapper.Map<IEnumerable<EmployeeShiftVm>>(employeeShifts);
    }
}