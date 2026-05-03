using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShiftHistories.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShiftHistories.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShiftHistories.UseCases.Queries.
    GetEmployeeShiftHistoryByEmployeeShiftId;

public class GetEmployeeShiftHistoryByEmployeeShiftIdQueryHandler(
    IMapper mapper,
    IEmployeeShiftHistoryRepository employeeShiftHistoryRepository)
    : IRequestHandler<GetEmployeeShiftHistoryByEmployeeShiftIdQuery, IEnumerable<EmployeeShiftHistoryVm>>
{
    public async Task<IEnumerable<EmployeeShiftHistoryVm>> Handle(GetEmployeeShiftHistoryByEmployeeShiftIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var employeeShiftHistory = await employeeShiftHistoryRepository
                .GetByEmployeeShiftId(request.EmployeeShiftId, cancellationToken);
        
        return mapper.Map<IEnumerable<EmployeeShiftHistoryVm>>(employeeShiftHistory);
    }
}