using ShiftTrack.Application.Modules.Organization.Timesheet.Common.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Commands.CreateEmployeeShifts;

public record CreateEmployeeShiftsCommand(
    IEnumerable<EmployeeShiftToCreateDto> Dtos) : IRequest<IEnumerable<EmployeeShiftVm>>;