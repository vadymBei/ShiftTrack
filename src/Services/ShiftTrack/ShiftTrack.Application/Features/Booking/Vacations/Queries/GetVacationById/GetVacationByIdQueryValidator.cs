using FluentValidation;

namespace ShiftTrack.Application.Features.Booking.Vacations.Queries.GetVacationById;

public class GetVacationByIdQueryValidator : AbstractValidator<GetVacationByIdQuery>
{
    public GetVacationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Id is required")
            .GreaterThan(0)
            .WithMessage("Id must be greater than zero.");
    }
}