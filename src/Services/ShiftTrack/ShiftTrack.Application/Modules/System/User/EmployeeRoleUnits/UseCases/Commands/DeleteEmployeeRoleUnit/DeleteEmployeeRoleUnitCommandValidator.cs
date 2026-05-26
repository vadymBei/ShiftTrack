using FluentValidation;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.UseCases.Commands.DeleteEmployeeRoleUnit;

public class DeleteEmployeeRoleUnitCommandValidator : AbstractValidator<DeleteEmployeeRoleUnitCommand>
{
    public DeleteEmployeeRoleUnitCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0.");
    }
}