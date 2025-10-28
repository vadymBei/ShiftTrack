using ShiftTrack.Application.Features.Organization.Timesheet.Common.Dtos;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Timesheet.EmployeeShifts.Commands.CreateEmployeeShifts;

public record CreateEmployeeShiftsCommand(
    IEnumerable<EmployeeShiftToCreateDto> Dtos) : IRequest<IEnumerable<EmployeeShiftVm>>;