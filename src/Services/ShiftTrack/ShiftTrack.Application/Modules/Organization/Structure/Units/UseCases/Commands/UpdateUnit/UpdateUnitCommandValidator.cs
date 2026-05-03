using FluentValidation;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Commands.UpdateUnit;

public class UpdateUnitCommandValidator : AbstractValidator<UpdateUnitCommand>
{
    public UpdateUnitCommandValidator()
    {
        RuleFor(x => x.Data.Id)
            .NotNull()
            .WithMessage("Id is required");

        RuleFor(x => x.Data.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Maximum field length is 100 characters");

        RuleFor(x => x.Data.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(100)
            .WithMessage("Maximum field length is 100 characters");

        RuleFor(x => x.Data.Code)
            .NotEmpty()
            .WithMessage("Code is required")
            .MaximumLength(10)
            .WithMessage("Maximum field length is 10 characters");
    }
}