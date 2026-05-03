using FluentValidation;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.UseCases.Commands.DeleteEmployeeRole;

public class DeleteEmployeeRoleCommandValidator : AbstractValidator<DeleteEmployeeRoleCommand>
{
    public DeleteEmployeeRoleCommandValidator()
    {
        RuleFor(command => command.EmployeeRoleId)
            .GreaterThan(0)
            .WithMessage("EmployeeRoleId must be greater than zero.");

    }
}