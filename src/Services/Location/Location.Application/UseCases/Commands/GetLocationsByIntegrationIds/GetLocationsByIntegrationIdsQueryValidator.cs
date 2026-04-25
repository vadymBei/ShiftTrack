using FluentValidation;

namespace Location.Application.UseCases.Commands.GetLocationsByIntegrationIds;

public class GetLocationsByIntegrationIdsQueryValidator : AbstractValidator<GetLocationsByIntegrationIdsQuery>
{
    public GetLocationsByIntegrationIdsQueryValidator()
    {
        
    }
}