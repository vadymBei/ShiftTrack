using ShiftTrack.Application.Modules.Organization.Payrolls.Dtos;

namespace ShiftTrack.Application.Modules.Organization.Payrolls.Interfaces;

public interface IPayrollService
{
    Task MarkPayrollAsPaid(MarkPayrollAsPaidDto dto, CancellationToken cancellationToken);
}