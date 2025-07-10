using FluentValidation;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnits.Commands.CreateEmployeeRoleUnit;

public class CreateEmployeeRoleUnitCommandValidator : AbstractValidator<CreateEmployeeRoleUnitCommand>
{
    public CreateEmployeeRoleUnitCommandValidator()
    {
        RuleFor(x => x.EmployeeRoleId)
            .GreaterThan(0)
            .WithMessage("EmployeeRoleId must be greater than 0.");
    
        RuleFor(x => x.UnitId)
            .GreaterThan(0)
            .WithMessage("UnitId must be greater than 0.");
    }
}