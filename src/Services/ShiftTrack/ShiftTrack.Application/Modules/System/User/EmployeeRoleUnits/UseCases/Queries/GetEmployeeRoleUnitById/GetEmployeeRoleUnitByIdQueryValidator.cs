using FluentValidation;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.UseCases.Queries.GetEmployeeRoleUnitById;

public class GetEmployeeRoleUnitByIdQueryValidator : AbstractValidator<GetEmployeeRoleUnitByIdQuery>
{
    public GetEmployeeRoleUnitByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0");
    }
}