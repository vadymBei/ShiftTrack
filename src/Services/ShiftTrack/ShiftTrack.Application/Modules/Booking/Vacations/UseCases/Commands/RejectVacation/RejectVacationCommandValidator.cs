using FluentValidation;

namespace ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Commands.RejectVacation;

public class RejectVacationCommandValidator : AbstractValidator<RejectVacationCommand>
{
    public RejectVacationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Id is required")
            .GreaterThan(0)
            .WithMessage("Id must be greater than zero.");
    }
}