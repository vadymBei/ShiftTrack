using Location.Application.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace Location.Application.UseCases.Commands.GetLocationsByIntegrationIds;

public record GetLocationsByIntegrationIdsQuery(
    IEnumerable<string> IntegrationIds) : IRequest<IEnumerable<LocationVm>>;