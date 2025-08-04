using FluentValidation;

namespace ShiftTrack.Application.Features.Booking.Vacations.Queries.GetVacations;

public class GetVacationsQueryValidator : AbstractValidator<GetVacationsQuery>
{
    public GetVacationsQueryValidator()
    {
        RuleFor(x => x.Filter.StartDate)
            .Must((query, startDate) => 
                !startDate.HasValue || !query.Filter.EndDate.HasValue || startDate.Value <= query.Filter.EndDate.Value)
            .When(x => x.Filter.StartDate.HasValue && x.Filter.EndDate.HasValue)
            .WithMessage("Start date must be less than or equal to end date");

        RuleFor(x => x.Filter.EndDate)
            .Must((query, endDate) => 
                !endDate.HasValue || !query.Filter.StartDate.HasValue || endDate.Value >= query.Filter.StartDate.Value)
            .When(x => x.Filter.StartDate.HasValue && x.Filter.EndDate.HasValue)
            .WithMessage("End date must be greater than or equal to start date");

        RuleFor(x => x.Filter.UnitId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Filter.DepartmentId)
            .NotEmpty()
            .GreaterThan(0);
    }
}