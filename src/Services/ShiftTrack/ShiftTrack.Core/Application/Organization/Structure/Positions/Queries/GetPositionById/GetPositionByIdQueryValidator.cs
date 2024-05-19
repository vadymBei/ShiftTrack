using FluentValidation;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Queries.GetPositionById
{
    public class GetPositionByIdQueryValidator : AbstractValidator<GetPositionByIdQuery>
    {
        public GetPositionByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                    .WithMessage("Id is required");
        }
    }
}
