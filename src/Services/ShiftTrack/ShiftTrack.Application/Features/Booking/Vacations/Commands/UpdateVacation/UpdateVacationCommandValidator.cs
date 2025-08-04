using FluentValidation;
using ShiftTrack.Domain.Features.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Features.Booking.Vacations.Commands.UpdateVacation;

public class UpdateVacationCommandValidator : AbstractValidator<UpdateVacationCommand>
{
    public UpdateVacationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .Must(startDate => startDate.Date >= DateTime.Now.Date)
            .WithMessage("Start date must not be in the past");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .Must((command, endDate) => endDate >= command.StartDate)
            .WithMessage("End date must be greater than or equal to start date");
            
        RuleFor(x => x.Type)
            .NotEqual(VacationType.None)
            .IsInEnum();
    }
}