using AutoMapper;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Extensions;

public static class VacationVmExtensions
{
    public static VacationVm ToVm(this Vacation vacation, IMapper mapper,
        IVacationPermissionService permissionService)
    {
        var vacationVm = mapper.Map<VacationVm>(vacation);
        vacationVm.Permissions = permissionService.GetPermissions(vacation);

        return vacationVm;
    }
}
