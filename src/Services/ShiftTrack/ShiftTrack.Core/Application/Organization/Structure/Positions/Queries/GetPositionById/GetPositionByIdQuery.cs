using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Queries.GetPositionById;

public record GetPositionByIdQuery(
    long Id) : IRequest<PositionVM>;