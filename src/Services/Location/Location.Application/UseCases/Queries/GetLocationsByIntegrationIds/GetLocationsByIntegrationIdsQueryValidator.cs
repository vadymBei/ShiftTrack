using FluentValidation;

namespace Location.Application.UseCases.Queries.GetLocationsByIntegrationIds;

public class GetLocationsByIntegrationIdsQueryValidator : AbstractValidator<GetLocationsByIntegrationIdsQuery>
{
    public GetLocationsByIntegrationIdsQueryValidator()
    {
        RuleFor(x => x.IntegrationIds)
            .NotEmpty().WithMessage("IntegrationIds collection must not be empty.")
            .ForEach(id => id.NotEmpty().WithMessage("Each IntegrationId must not be empty."));
    }
}