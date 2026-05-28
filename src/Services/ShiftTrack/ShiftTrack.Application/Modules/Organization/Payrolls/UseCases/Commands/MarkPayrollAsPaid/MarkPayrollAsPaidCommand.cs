using ShiftTrack.Application.Modules.Organization.Payrolls.Dtos;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Payrolls.UseCases.Commands.MarkPayrollAsPaid;

public record MarkPayrollAsPaidCommand(
    MarkPayrollAsPaidDto Data) : IRequest;