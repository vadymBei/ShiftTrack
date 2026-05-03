using Location.Application.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace Location.Application.UseCases.Queries.GetLocationsByIntegrationIds;

public record GetLocationsByIntegrationIdsQuery(
    IEnumerable<string> IntegrationIds) : IRequest<IEnumerable<LocationVm>>;