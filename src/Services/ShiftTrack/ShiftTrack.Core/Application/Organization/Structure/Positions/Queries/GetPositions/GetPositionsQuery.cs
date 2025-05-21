using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Queries.GetPositions;

public record GetPositionsQuery() : IRequest<IEnumerable<PositionVM>>;