using FluentValidation;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Queries.GetUnitById
{
    public class GetUnitByIdQueryValidator : AbstractValidator<GetUnitByIdQuery>
    {
        public GetUnitByIdQueryValidator()
        {
            RuleFor(x => x.Id)
               .NotNull()
                   .WithMessage("Id is required");
        }
    }
}
