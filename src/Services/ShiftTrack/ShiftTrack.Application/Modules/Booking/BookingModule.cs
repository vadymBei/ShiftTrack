using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Services;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.Services;
using ShiftTrack.Application.Modules.Booking.Vacations.Strategies;

namespace ShiftTrack.Application.Modules.Booking;

public static class BookingModule
{
    public static IServiceCollection AddBookingServices(this IServiceCollection services)
    {
        //BusinessTrips
        services.AddTransient<IBusinessTripService, BusinessTripService>();
        services.AddTransient<IBusinessTripRouteService, BusinessTripRouteService>();
        
        //Vacations
        services.AddTransient<IVacationShiftService, VacationShiftService>();
        services.AddTransient<IVacationStrategyFactory, VacationStrategyFactory>();
        services.AddTransient<IVacationStrategy, DefaultVacationStrategy>();
        services.AddTransient<IVacationStrategy, DepartmentDirectorVacationStrategy>();
        services.AddTransient<IVacationStrategy, SysAdminVacationStrategy>();
        services.AddTransient<IVacationStrategy, UnitDirectorVacationStrategy>();
        services.AddTransient<IVacationService, VacationService>();
        services.AddTransient<IVacationPermissionService, VacationPermissionService>();
        services.AddTransient<IVacationAccrualCalculator, VacationAccrualCalculator>();
        services.AddTransient<IVacationAccrualService, VacationAccrualService>();

        return services;
    }
}