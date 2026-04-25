using Location.Application.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace Location.Application.UseCases.Commands.SearchLocations;

public record SearchLocationsCommand(
    string SearchTerm) : IRequest<IEnumerable<LocationVm>>; 