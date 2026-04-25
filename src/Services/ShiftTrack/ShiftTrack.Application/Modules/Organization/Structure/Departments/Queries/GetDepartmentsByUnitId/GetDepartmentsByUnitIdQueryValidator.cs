using FluentValidation;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.Queries.GetDepartmentsByUnitId;

public class GetDepartmentsByUnitIdQueryValidator : AbstractValidator<GetDepartmentsByUnitIdQuery>
{
    public GetDepartmentsByUnitIdQueryValidator()
    {
        RuleFor(x => x.UnitId)
            .NotEmpty()
            .WithMessage("UnitId is required")
            .NotNull()
            .WithMessage("UnitId is required")
            .GreaterThan(0)
            .WithMessage("UnitId must be bigger than 0");
    }
}