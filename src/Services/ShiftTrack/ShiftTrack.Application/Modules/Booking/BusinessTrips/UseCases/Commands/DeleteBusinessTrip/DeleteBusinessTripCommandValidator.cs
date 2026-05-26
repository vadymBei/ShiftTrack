using FluentValidation;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.DeleteBusinessTrip;

public class DeleteBusinessTripCommandValidator : AbstractValidator<DeleteBusinessTripCommand>
{
    public DeleteBusinessTripCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}