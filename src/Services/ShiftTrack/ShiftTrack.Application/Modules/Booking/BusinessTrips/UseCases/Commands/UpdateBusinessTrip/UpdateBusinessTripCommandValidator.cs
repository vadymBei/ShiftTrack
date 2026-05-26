using FluentValidation;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.UseCases.Commands.UpdateBusinessTrip;

public class UpdateBusinessTripCommandValidator : AbstractValidator<UpdateBusinessTripCommand>
{
    public UpdateBusinessTripCommandValidator()
    {
        RuleFor(x => x.Data.Id)
            .GreaterThan(0);
        
        RuleFor(x => x.Data.StartDate)
            .NotEmpty()
            .Must(x => x.Date >= DateTime.Today)
            .WithMessage("Start date must be in the future or today");

        RuleFor(x => x.Data.EndDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.Data.StartDate)
            .WithMessage("End date must be greater than or equal to start date");

        RuleFor(x => x.Data.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Data.EstimatedBudget)
            .GreaterThan(0);

        RuleFor(x => x.Data.EmployeeIds)
            .NotEmpty()
            .WithMessage("At least one employee must be specified");

        RuleFor(x => x.Data.LocationIntegrationIds)
            .NotEmpty()
            .WithMessage("At least one location must be specified");
    }
}