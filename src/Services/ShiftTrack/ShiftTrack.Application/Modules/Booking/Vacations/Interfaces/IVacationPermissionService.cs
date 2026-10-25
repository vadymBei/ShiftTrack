using ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;

public interface IVacationPermissionService
{
    VacationPermissionsVm GetPermissions(Vacation vacation);
    bool CanEdit(Vacation vacation);
    bool CanDelete(Vacation vacation);
}
