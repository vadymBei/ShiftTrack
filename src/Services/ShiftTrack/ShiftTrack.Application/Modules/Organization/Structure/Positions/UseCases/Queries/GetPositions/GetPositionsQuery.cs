using ShiftTrack.Application.Modules.Organization.Structure.Positions.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Queries.GetPositions;

public record GetPositionsQuery() : IRequest<IEnumerable<PositionVm>>;