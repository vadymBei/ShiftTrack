using FluentValidation;

namespace ShiftTrack.Application.Features.Booking.Vacations.Commands.DeleteVacation;

public class DeleteVacationCommandValidator : AbstractValidator<DeleteVacationCommand>
{
    public DeleteVacationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Id is required")
            .GreaterThan(0)
            .WithMessage("Id must be greater than zero.");
    }
}