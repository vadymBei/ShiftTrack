using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Positions.Queries.GetPositions;

public record GetPositionsQuery() : IRequest<IEnumerable<PositionVm>>;