using FluentValidation;

namespace ShiftTrack.Application.Features.Booking.Vacations.Commands.ApproveVacation;

public class ApproveVacationCommandValidator : AbstractValidator<ApproveVacationCommand>
{
    public ApproveVacationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Id is required")
            .GreaterThan(0)
            .WithMessage("Id must be greater than zero.");
    }
}