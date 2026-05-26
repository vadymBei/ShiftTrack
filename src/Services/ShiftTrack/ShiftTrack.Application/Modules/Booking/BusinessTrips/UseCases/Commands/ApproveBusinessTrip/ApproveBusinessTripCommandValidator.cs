using FluentValidation;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.ApproveBusinessTrip;

public class ApproveBusinessTripCommandValidator : AbstractValidator<ApproveBusinessTripCommand>
{
    public ApproveBusinessTripCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}