using FluentValidation;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnitDepartments.Queries.GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitId;

public class GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitIdQueryValidator : AbstractValidator<GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitIdQuery>
{
    public GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitIdQueryValidator()
    {
        RuleFor(x => x.EmployeeRoleUnitId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("EmployeeRoleUnitId must be greater than 0");
    }
}