using FluentValidation;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Queries.GetBusinessTripById;

public class GetBusinessTripByIdQueryValidator : AbstractValidator<GetBusinessTripByIdQuery>
{
    public GetBusinessTripByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}