using FluentValidation;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnitDepartments.Commands.CreateEmployeeRoleUnitDepartment;

public class CreateEmployeeRoleUnitDepartmentCommandValidator : AbstractValidator<CreateEmployeeRoleUnitDepartmentCommand>
{
    public CreateEmployeeRoleUnitDepartmentCommandValidator()
    {
        RuleFor(x => x.EmployeeRoleUnitId)
            .GreaterThan(0)
            .WithMessage("EmployeeRoleUnitId must be greater than 0");
    
        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage("DepartmentId must be greater than 0");
    }
}