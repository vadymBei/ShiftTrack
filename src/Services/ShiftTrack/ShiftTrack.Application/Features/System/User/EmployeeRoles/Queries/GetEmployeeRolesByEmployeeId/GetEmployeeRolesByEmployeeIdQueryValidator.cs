using FluentValidation;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoles.Queries.GetEmployeeRolesByEmployeeId;

public class GetEmployeeRolesByEmployeeIdQueryValidator : AbstractValidator<GetEmployeeRolesByEmployeeIdQuery>
{
    public GetEmployeeRolesByEmployeeIdQueryValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage("EmployeeId must be greater than 0");
    }
}