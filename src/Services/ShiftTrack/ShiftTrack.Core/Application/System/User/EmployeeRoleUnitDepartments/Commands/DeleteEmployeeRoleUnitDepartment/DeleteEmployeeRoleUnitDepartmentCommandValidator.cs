using FluentValidation;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnitDepartments.Commands.DeleteEmployeeRoleUnitDepartment;

public class DeleteEmployeeRoleUnitDepartmentCommandValidator : AbstractValidator<DeleteEmployeeRoleUnitDepartmentCommand>
{
    public DeleteEmployeeRoleUnitDepartmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0.");
    }
}