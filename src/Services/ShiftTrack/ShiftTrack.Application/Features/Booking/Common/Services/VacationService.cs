using ShiftTrack.Application.Features.Booking.Common.Dtos;
using ShiftTrack.Application.Features.Booking.Common.Interfaces;
using ShiftTrack.Domain.Features.Booking.Vacations.Entities;

namespace ShiftTrack.Application.Features.Booking.Common.Services;

public class VacationService(
    IVacationStrategyFactory vacationStrategyFactory) : IVacationService
{
    private IVacationStrategy VacationStrategy =>
        vacationStrategyFactory.GetStrategy();
    
    public async Task<Vacation> GetById(object id, CancellationToken cancellationToken)
    {
        return await VacationStrategy.GetVacationById((long)id, cancellationToken);
    }

    public async Task<Vacation> ApproveVacation(long id, CancellationToken cancellationToken)
    {
        await VacationStrategy.ApproveVacation(id, cancellationToken);
        
        return await VacationStrategy.GetVacationById(id, cancellationToken);
    }

    public Task<IEnumerable<Vacation>> GetVacations(VacationsFilterDto filter, CancellationToken cancellationToken)
    {
        return VacationStrategy.GetVacations(filter, cancellationToken);
    }

    public async Task<Vacation> RejectVacation(long id, CancellationToken cancellationToken)
    {
        await VacationStrategy.RejectVacation(id, cancellationToken);
        
        return await VacationStrategy.GetVacationById(id, cancellationToken);
    }
}