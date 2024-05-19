using FluentValidation;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Commands.DeleteUnit
{
    public class DeleteUnitCommandValidator : AbstractValidator<DeleteUnitCommand>
    {
        public DeleteUnitCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                    .WithMessage("Id is required");
        }
    }
}
