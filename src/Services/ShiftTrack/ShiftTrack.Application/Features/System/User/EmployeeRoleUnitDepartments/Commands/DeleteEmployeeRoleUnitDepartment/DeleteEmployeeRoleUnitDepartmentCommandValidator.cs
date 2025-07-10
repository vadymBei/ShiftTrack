using FluentValidation;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnitDepartments.Commands.DeleteEmployeeRoleUnitDepartment;

public class DeleteEmployeeRoleUnitDepartmentCommandValidator : AbstractValidator<DeleteEmployeeRoleUnitDepartmentCommand>
{
    public DeleteEmployeeRoleUnitDepartmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0.");
    }
}