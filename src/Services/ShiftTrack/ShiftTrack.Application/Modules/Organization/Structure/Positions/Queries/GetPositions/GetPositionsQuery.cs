using ShiftTrack.Application.Modules.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.Queries.GetPositions;

public record GetPositionsQuery() : IRequest<IEnumerable<PositionVm>>;