using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Application.Features.Booking.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Common.Services;

namespace ShiftTrack.Application.Features.Booking;

public static class BookingModule
{
    public static IServiceCollection AddBookingServices(this IServiceCollection services)
    {
        //Vacations
        services.AddTransient<IVacationService, VacationService>();
        
        return services;
    }
}