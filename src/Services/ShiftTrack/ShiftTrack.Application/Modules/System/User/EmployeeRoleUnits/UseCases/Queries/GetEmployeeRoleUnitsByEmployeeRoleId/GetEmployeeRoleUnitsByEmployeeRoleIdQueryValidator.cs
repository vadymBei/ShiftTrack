using FluentValidation;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.UseCases.Queries.GetEmployeeRoleUnitsByEmployeeRoleId;

public class GetEmployeeRoleUnitsByEmployeeRoleIdQueryValidator : AbstractValidator<GetEmployeeRoleUnitsByEmployeeRoleIdQuery>
{
    public GetEmployeeRoleUnitsByEmployeeRoleIdQueryValidator()
    {
        RuleFor(x => x.EmployeeRoleId)
            .GreaterThan(0)
            .WithMessage("EmployeeRoleId must be greater than 0");
    }
}