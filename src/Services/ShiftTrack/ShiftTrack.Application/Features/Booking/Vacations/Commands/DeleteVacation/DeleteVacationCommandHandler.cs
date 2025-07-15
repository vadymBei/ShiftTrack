using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Common.Constants;
using ShiftTrack.Application.Features.Booking.Common.Exceptions;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Domain.Features.Booking.Vacations.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Booking.Vacations.Commands.DeleteVacation;

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
            && vacation.AuthorId != currentUserService.Employee.Id)
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_DELETE_OTHERS_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_DELETE_OTHERS_VACATION));
        }
        
        applicationDbContext.Vacations.Remove(vacation);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}