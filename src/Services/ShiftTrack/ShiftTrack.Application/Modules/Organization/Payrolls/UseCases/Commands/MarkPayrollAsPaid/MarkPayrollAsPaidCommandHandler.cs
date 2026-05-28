using ShiftTrack.Application.Modules.Organization.Payrolls.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Payrolls.UseCases.Commands.MarkPayrollAsPaid;

public class MarkPayrollAsPaidCommandHandler(
    IPayrollService payrollService) : IRequestHandler<MarkPayrollAsPaidCommand>
{
    public async Task Handle(MarkPayrollAsPaidCommand request, CancellationToken cancellationToken = default)
    {
        await payrollService.MarkPayrollAsPaid(request.Data, cancellationToken);
    }
}