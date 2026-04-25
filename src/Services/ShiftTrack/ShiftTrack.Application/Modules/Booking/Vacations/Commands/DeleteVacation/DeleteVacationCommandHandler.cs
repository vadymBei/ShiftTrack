using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.Common.Constants;
using ShiftTrack.Application.Modules.Booking.Common.Exceptions;
using ShiftTrack.Application.Modules.System.User.Common.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Commands.DeleteVacation;

public class DeleteVacationCommandHandler(
    ICurrentUserService currentUserService,
    IEmployeeRoleChecker employeeRoleChecker,
    IApplicationDbContext applicationDbContext) : IRequestHandler<DeleteVacationCommand>
{
    public async Task Handle(DeleteVacationCommand request, CancellationToken cancellationToken = default)
    {
        var vacation = await applicationDbContext.Vacations
                           .FindAsync([request.Id], cancellationToken: cancellationToken)
                       ?? throw new EntityNotFoundException(typeof(Vacation), request.Id);

        if (!employeeRoleChecker.HasCurrentUserSysAdminRole()
            && vacation.AuthorId != currentUserService.Employee.Id
            && vacation.Status == VacationStatus.PendingApproval)
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_DELETE_OTHERS_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_DELETE_OTHERS_VACATION));
        }
        
        applicationDbContext.Vacations.Remove(vacation);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}