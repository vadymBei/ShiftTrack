using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShiftHistories.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShiftHistories.UseCases.Queries.GetEmployeeShiftHistoryByEmployeeShiftId;

public record GetEmployeeShiftHistoryByEmployeeShiftIdQuery(
    long EmployeeShiftId) : IRequest<IEnumerable<EmployeeShiftHistoryVm>>;