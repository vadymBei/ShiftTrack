using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Queries.GetPositions;

public record GetPositionsQuery() : IRequest<IEnumerable<PositionVM>>;