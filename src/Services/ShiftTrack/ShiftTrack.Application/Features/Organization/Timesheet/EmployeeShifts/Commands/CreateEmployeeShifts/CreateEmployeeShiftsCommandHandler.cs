using AutoMapper;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Timesheet.EmployeeShifts.Commands.CreateEmployeeShifts;

public class CreateEmployeeShiftsCommandHandler(
    IMapper mapper,
    IEmployeeShiftService employeeShiftService)
    : IRequestHandler<CreateEmployeeShiftsCommand, IEnumerable<EmployeeShiftVm>>
{
    public async Task<IEnumerable<EmployeeShiftVm>> Handle(CreateEmployeeShiftsCommand request,
        CancellationToken cancellationToken = default)
    {
        var employeeShifts = await employeeShiftService
            .CreateEmployeeShifts(request.Dtos, cancellationToken);

        return mapper.Map<IEnumerable<EmployeeShiftVm>>(employeeShifts);
    }
}