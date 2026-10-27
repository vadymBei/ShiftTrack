using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.Constants;
using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;
using ShiftTrack.Application.Modules.Booking.Vacations.Exceptions;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Strategies;

public class DefaultVacationStrategy(
    IVacationRepository vacationRepository,
    ICurrentUserService currentUserService) : IVacationStrategy
{
    public async Task ApproveVacation(long id, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetForUpdate(id, cancellationToken);

        if (vacation.AuthorId != currentUserService.Employee.Id)
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_VIEW_OTHERS_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_VIEW_OTHERS_VACATION));
        }
        
        if (!vacation.CanSubmit)
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_APPROVE_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_APPROVE_VACATION));
        }

        vacation.Submit();
        await vacationRepository.Save(cancellationToken);
    }

    public Task RejectVacation(long id, CancellationToken cancellationToken)
    {
        throw new VacationException(VacationExceptionsLocalization.CANNOT_REJECT_VACATION,
            nameof(VacationExceptionsLocalization.CANNOT_REJECT_VACATION));
    }

    public async Task<Vacation> GetVacationById(long id, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetById(id, cancellationToken);

        if (vacation.AuthorId != currentUserService.Employee.Id)
        {
            throw new VacationException(VacationExceptionsLocalization.CANNOT_VIEW_OTHERS_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_VIEW_OTHERS_VACATION));
        }

        return vacation;
    }

    public VacationApprovalFilterDto GetApprovalFilter() => null;

    public VacationAccessDto GetAccess(Vacation vacation)
    {
        var isAuthor = vacation.AuthorId == currentUserService.Employee.Id;

        return new VacationAccessDto(
            CanView: isAuthor,
            CanApprove: isAuthor && vacation.CanSubmit,
            CanReject: false,
            ApproveSubmitsForApproval: true);
    }

    /// <summary>Own vacations and vacations of colleagues from the same department.</summary>
    public VacationAccessFilterDto GetAccessFilter()
    {
        var currentEmployee = currentUserService.Employee;

        return new VacationAccessFilterDto(
            [],
            currentEmployee.DepartmentId is { } departmentId ? [departmentId] : [],
            currentEmployee.Id);
    }
}