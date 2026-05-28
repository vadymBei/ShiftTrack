using ShiftTrack.Application.Modules.Organization.Payrolls.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Payrolls.UseCases.Queries.GetPayrolls;

public record GetPayrollsQuery(
    DateTime Period,
    long DepartmentId) : IRequest<PayrollSummaryVm>;