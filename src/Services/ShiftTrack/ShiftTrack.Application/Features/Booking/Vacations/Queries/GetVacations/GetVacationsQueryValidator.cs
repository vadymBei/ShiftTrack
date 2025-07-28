using FluentValidation;

namespace ShiftTrack.Application.Features.Booking.Vacations.Queries.GetVacations;

public class GetVacationsQueryValidator : AbstractValidator<GetVacationsQuery>
{
    public GetVacationsQueryValidator()
    {
        RuleFor(x => x.StartDate)
            .Must((query, startDate) => 
                !startDate.HasValue || !query.EndDate.HasValue || startDate.Value <= query.EndDate.Value)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("Start date must be less than or equal to end date");

        RuleFor(x => x.EndDate)
            .Must((query, endDate) => 
                !endDate.HasValue || !query.StartDate.HasValue || endDate.Value >= query.StartDate.Value)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("End date must be greater than or equal to start date");

        RuleFor(x => x.UnitId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .GreaterThan(0);
    }
}