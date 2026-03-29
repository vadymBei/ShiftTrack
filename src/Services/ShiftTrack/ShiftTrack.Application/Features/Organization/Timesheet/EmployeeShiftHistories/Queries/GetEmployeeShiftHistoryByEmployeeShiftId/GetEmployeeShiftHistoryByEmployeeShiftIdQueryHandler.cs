using AutoMapper;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Timesheet.EmployeeShiftHistories.Queries.
    GetEmployeeShiftHistoryByEmployeeShiftId;

public class GetEmployeeShiftHistoryByEmployeeShiftIdQueryHandler(
    IMapper mapper,
    IEmployeeShiftHistoryService employeeShiftHistoryService)
    : IRequestHandler<GetEmployeeShiftHistoryByEmployeeShiftIdQuery, IEnumerable<EmployeeShiftHistoryVm>>
{
    public async Task<IEnumerable<EmployeeShiftHistoryVm>> Handle(GetEmployeeShiftHistoryByEmployeeShiftIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var employeeShiftHistory = await employeeShiftHistoryService
                .GetByEmployeeShiftId(request.EmployeeShiftId, cancellationToken);
        
        return mapper.Map<IEnumerable<EmployeeShiftHistoryVm>>(employeeShiftHistory);
    }
}