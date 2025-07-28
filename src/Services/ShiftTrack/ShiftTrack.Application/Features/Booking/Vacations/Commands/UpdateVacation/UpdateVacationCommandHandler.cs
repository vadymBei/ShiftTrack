using AutoMapper;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Common.Constants;
using ShiftTrack.Application.Features.Booking.Common.Exceptions;
using ShiftTrack.Application.Features.Booking.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Employees.Common.Interfaces;
using ShiftTrack.Domain.Features.Booking.Vacations.Entities;
using ShiftTrack.Domain.Features.Booking.Vacations.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Booking.Vacations.Commands.UpdateVacation;

public class UpdateVacationCommandHandler(
    IMapper mapper,
    IEmployeeService employeeService,
    IVacationService vacationService,
    ICurrentUserService currentUserService,
    IApplicationDbContext applicationDbContext) : IRequestHandler<UpdateVacationCommand, VacationVm>
{
    public async Task<VacationVm> Handle(UpdateVacationCommand request, CancellationToken cancellationToken = default)
    {
        var vacation = await GetAndValidateVacationForUpdate(request.Id, cancellationToken);

        if (vacation.StartDate != request.StartDate
            || vacation.EndDate != request.EndDate)
        {
            vacation.StartDate = request.StartDate;
            vacation.EndDate = request.EndDate;

            var vacationDaysCount = (request.EndDate - request.StartDate).Days + 1;

            if (vacation.DaysCount != vacationDaysCount)
            {
                var employee = await employeeService
                    .GetById(vacation.EmployeeId, cancellationToken);

                var vacationDaysBalance = employee.VacationDaysBalance + vacation.DaysCount;

                vacation.DaysCount = vacationDaysCount;
                vacation.DaysBalanceAtCreation = vacationDaysBalance;
            }
        }

        vacation.Comment = request.Comment;
        vacation.Type = request.Type;

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        vacation = await vacationService.GetById(vacation.Id, cancellationToken);
        
        return mapper.Map<VacationVm>(vacation);
    }

    private async Task<Vacation> GetAndValidateVacationForUpdate (long id, CancellationToken cancellationToken)
    {
        var vacation = await applicationDbContext.Vacations.FindAsync(
                           new object[] { id },
                           cancellationToken: cancellationToken)
                       ?? throw new EntityNotFoundException(typeof(Vacation), id);

        if (vacation.AuthorId != currentUserService.Employee.Id)
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_EDIT_OTHERS_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_EDIT_OTHERS_VACATION));

        if (vacation.Status != VacationStatus.PendingApproval)
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_EDIT_NON_PENDING_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_EDIT_NON_PENDING_VACATION));

        return vacation;
    }
}