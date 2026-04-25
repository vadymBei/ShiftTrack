using FluentValidation;

namespace Location.Application.UseCases.Commands.SearchLocations;

public class SearchLocationsCommandValidator : AbstractValidator<SearchLocationsCommand>
{
    public SearchLocationsCommandValidator()
    {
        RuleFor(x => x.SearchTerm)
            .NotEmpty()
            .WithMessage("Search term is required.")
            .MinimumLength(2)
            .WithMessage("Search term must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Search term must not exceed 100 characters.");
    }
}