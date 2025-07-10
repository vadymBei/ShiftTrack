using FluentValidation;

namespace ShiftTrack.Application.Features.Organization.Structure.Positions.Queries.GetPositionById;

public class GetPositionByIdQueryValidator : AbstractValidator<GetPositionByIdQuery>
{
    public GetPositionByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Id is required");
    }
}