using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.UseCases.Queries.GetEmployeeShiftsByEmployeeId;

public record GetEmployeeShiftsByEmployeeIdQuery(
    EmployeeShiftsByEmployeeIdDto Data) : IRequest<IEnumerable<EmployeeShiftVm>>;