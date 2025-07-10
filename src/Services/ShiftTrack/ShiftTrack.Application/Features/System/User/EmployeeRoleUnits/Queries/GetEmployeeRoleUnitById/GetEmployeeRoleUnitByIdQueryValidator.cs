using FluentValidation;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnits.Queries.GetEmployeeRoleUnitById;

public class GetEmployeeRoleUnitByIdQueryValidator : AbstractValidator<GetEmployeeRoleUnitByIdQuery>
{
    public GetEmployeeRoleUnitByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0");
    }
}