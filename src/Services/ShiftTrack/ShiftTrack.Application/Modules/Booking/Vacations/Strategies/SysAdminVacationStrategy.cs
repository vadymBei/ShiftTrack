using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.Constants;
using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;
using ShiftTrack.Application.Modules.Booking.Vacations.Exceptions;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Strategies;

public class SysAdminVacationStrategy(
    ICurrentUserService currentUserService,
    IVacationRepository vacationRepository,
    IEmployeeShiftService employeeShiftService,
    IVacationShiftService vacationShiftService) : IVacationStrategy
{
    public async Task ApproveVacation(long id, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetForUpdate(id, cancellationToken);

        if (IsOwnDraft(vacation))
        {
            vacation.Submit();
            await vacationRepository.Save(cancellationToken);
            return;
        }

        EnsureCanApprove(vacation);

        vacation.Approve(VacationApprovalLevel.UnitDirector);
        await vacationRepository.Save(cancellationToken);

        await vacationShiftService.SetVacationShifts(id, cancellationToken);
    }

    public async Task RejectVacation(long id, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetForUpdate(id, cancellationToken);

        EnsureCanReject(vacation);

        vacation.Reject(VacationApprovalLevel.UnitDirector);
        await vacationRepository.Save(cancellationToken);

        await employeeShiftService.RestorePreviousEmployeeShifts(
            new RestoreEmployeeShiftsDto(
                [vacation.EmployeeId],
                vacation.StartDate,
                vacation.EndDate),
            cancellationToken);
    }

    public async Task<Vacation> GetVacationById(long id, CancellationToken cancellationToken)
    {
        var vacation = await vacationRepository.GetById(id, cancellationToken);

        return vacation;
    }

    public VacationAccessDto GetAccess(Vacation vacation)
    {
        var isOwnDraft = IsOwnDraft(vacation);

        return new VacationAccessDto(
            CanView: true,
            CanApprove: isOwnDraft || vacation.CanApprove(VacationApprovalLevel.UnitDirector),
            CanReject: vacation.CanReject(VacationApprovalLevel.UnitDirector),
            ApproveSubmitsForApproval: isOwnDraft);
    }

    private bool IsOwnDraft(Vacation vacation) =>
        vacation.AuthorId == currentUserService.Employee.Id && vacation.CanSubmit;

    private void EnsureCanApprove(Vacation vacation)
    {
        if (!GetAccess(vacation).CanApprove)
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_APPROVE_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_APPROVE_VACATION));
        }
    }

    private void EnsureCanReject(Vacation vacation)
    {
        if (!GetAccess(vacation).CanReject)
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_REJECT_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_REJECT_VACATION));
        }
    }

    public VacationApprovalFilterDto GetApprovalFilter()
    {
        return new VacationApprovalFilterDto(
            [VacationStatus.PendingApproval, VacationStatus.ApprovedByDepartmentDirector],
            null,
            null,
            currentUserService.Employee.Id);
    }

    public VacationAccessFilterDto GetAccessFilter() => VacationAccessFilterDto.Unrestricted;
}