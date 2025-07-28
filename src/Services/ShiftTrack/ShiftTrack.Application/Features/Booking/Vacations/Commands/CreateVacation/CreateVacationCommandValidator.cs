using FluentValidation;
using ShiftTrack.Domain.Features.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Features.Booking.Vacations.Commands.CreateVacation;

public class CreateVacationCommandValidator : AbstractValidator<CreateVacationCommand>
{
    public CreateVacationCommandValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .Must(x => x.Date >= DateTime.Today)
            .WithMessage("Start date must be in the future or today");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be greater than or equal to start date");

        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Type)
            .NotEqual(VacationType.None)
            .IsInEnum();
    }
}