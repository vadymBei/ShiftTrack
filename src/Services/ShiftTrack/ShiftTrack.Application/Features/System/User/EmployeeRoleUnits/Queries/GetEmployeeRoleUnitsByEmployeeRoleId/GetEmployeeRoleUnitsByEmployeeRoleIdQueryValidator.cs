using FluentValidation;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnits.Queries.GetEmployeeRoleUnitsByEmployeeRoleId;

public class GetEmployeeRoleUnitsByEmployeeRoleIdQueryValidator : AbstractValidator<GetEmployeeRoleUnitsByEmployeeRoleIdQuery>
{
    public GetEmployeeRoleUnitsByEmployeeRoleIdQueryValidator()
    {
        RuleFor(x => x.EmployeeRoleId)
            .GreaterThan(0)
            .WithMessage("EmployeeRoleId must be greater than 0");
    }
}