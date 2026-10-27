using Quartz;
using ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Commands.RecalculateVacationAccruals;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Scheduling.Jobs;

namespace ShiftTrack.Jobs.Jobs.Booking;

/// <summary>
/// Accrues annual vacation days of all employees by the Law of Ukraine "On Vacations".
/// Recalculates everything from the employment start, so it's safe to run any number of times.
/// </summary>
public class VacationAccrualJob(
    IMediator mediator,
    ILogger<VacationAccrualJob> logger) : ScheduledJob(logger)
{
    protected override async Task ExecuteJob(IJobExecutionContext context, CancellationToken cancellationToken)
    {
        var employeesCount = await mediator.Send(new RecalculateVacationAccrualsCommand(), cancellationToken);

        logger.LogInformation("Vacation accruals recalculated for {EmployeesCount} employees", employeesCount);
    }
}
