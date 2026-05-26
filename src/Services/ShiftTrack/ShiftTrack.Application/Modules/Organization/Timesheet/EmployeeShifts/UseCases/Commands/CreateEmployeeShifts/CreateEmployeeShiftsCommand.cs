using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.UseCases.Commands.CreateEmployeeShifts;

public record CreateEmployeeShiftsCommand(
    IEnumerable<EmployeeShiftToCreateDto> Dtos) : IRequest<IEnumerable<EmployeeShiftVm>>;