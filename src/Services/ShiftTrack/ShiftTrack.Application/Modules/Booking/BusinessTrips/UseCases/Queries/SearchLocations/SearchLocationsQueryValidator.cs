using FluentValidation;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Queries.SearchLocations;

public class SearchLocationsQueryValidator : AbstractValidator<SearchLocationsQuery>
{
    public SearchLocationsQueryValidator()
    {
        RuleFor(x => x.SearchTerm)
            .NotEmpty()
            .Must(x => x?.Trim().Length >= 2)
            .WithMessage("Search term must be at least 2 characters long.")
            .MaximumLength(100);
    }
}
