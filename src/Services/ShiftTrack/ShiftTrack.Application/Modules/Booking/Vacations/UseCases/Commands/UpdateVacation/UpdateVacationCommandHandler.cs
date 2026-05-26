using AutoMapper;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.Constants;
using ShiftTrack.Application.Modules.Booking.Vacations.Exceptions;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Commands.UpdateVacation;

public class UpdateVacationCommandHandler(
    IMapper mapper,
    IEmployeeRepository employeeRepository,
    IVacationService vacationService,
    IVacationRepository vacationRepository,
    ICurrentUserService currentUserService) : IRequestHandler<UpdateVacationCommand, VacationVm>
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
                var employee = await employeeRepository
                    .GetById(vacation.EmployeeId, cancellationToken);

                var vacationDaysBalance = employee.VacationDaysBalance + vacation.DaysCount;

                vacation.DaysCount = vacationDaysCount;
                vacation.DaysBalanceAtCreation = vacationDaysBalance;
            }
        }

        vacation.Comment = request.Comment;
        vacation.Type = request.Type;

        await vacationRepository.Update(vacation, cancellationToken);

        vacation = await vacationService.GetById(vacation.Id, cancellationToken);
        
        return mapper.Map<VacationVm>(vacation);
    }

    private async Task<Vacation> GetAndValidateVacationForUpdate (long id, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetById(id, cancellationToken);

        if (vacation.AuthorId != currentUserService.Employee.Id)
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_EDIT_OTHERS_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_EDIT_OTHERS_VACATION));

        if (vacation.Status != VacationStatus.None)
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_EDIT_NON_PENDING_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_EDIT_NON_PENDING_VACATION));

        return vacation;
    }
}