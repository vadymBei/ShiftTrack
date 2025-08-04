using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Application.Features.Booking.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Common.Services;
using ShiftTrack.Application.Features.Booking.Common.Strategies;

namespace ShiftTrack.Application.Features.Booking;

public static class BookingModule
{
    public static IServiceCollection AddBookingServices(this IServiceCollection services)
    {
        //Vacations
        services.AddTransient<ICommonVacationService, CommonVacationService>();
        services.AddTransient<IVacationStrategyFactory, VacationStrategyFactory>();
        services.AddTransient<IVacationStrategy, VacationDefaultStrategy>();
        services.AddTransient<IVacationStrategy, VacationDepartmentDirectorStrategy>();
        services.AddTransient<IVacationStrategy, VacationSysAdminStrategy>();
        services.AddTransient<IVacationStrategy, VacationUnitDirectorStrategy>();
        services.AddTransient<IVacationService, VacationService>();
        
        return services;
    }
}