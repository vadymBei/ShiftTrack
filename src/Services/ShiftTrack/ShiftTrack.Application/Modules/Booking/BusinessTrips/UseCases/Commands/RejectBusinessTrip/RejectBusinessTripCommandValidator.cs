using FluentValidation;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.RejectBusinessTrip;

public class RejectBusinessTripCommandValidator : AbstractValidator<RejectBusinessTripCommand>
{
    public RejectBusinessTripCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}