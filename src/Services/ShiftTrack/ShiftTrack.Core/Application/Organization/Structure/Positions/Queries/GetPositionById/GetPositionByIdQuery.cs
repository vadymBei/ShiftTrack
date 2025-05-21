using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Queries.GetPositionById;

public record GetPositionByIdQuery(
    long Id) : IRequest<PositionVM>;