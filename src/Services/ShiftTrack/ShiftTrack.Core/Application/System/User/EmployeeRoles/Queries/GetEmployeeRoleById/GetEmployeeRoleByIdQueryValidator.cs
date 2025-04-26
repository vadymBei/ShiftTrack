using FluentValidation;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoles.Queries.GetEmployeeRoleById;

public class GetEmployeeRoleByIdQueryValidator : AbstractValidator<GetEmployeeRoleByIdQuery>
{
    public GetEmployeeRoleByIdQueryValidator()
    {
        RuleFor(x => x.EmployeeRoleId)
            .GreaterThan(0)
            .WithMessage("EmployeeRoleId must be greater than 0");
    }
}