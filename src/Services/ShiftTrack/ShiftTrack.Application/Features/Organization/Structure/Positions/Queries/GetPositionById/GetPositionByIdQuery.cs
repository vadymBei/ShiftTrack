using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Positions.Queries.GetPositionById;

public record GetPositionByIdQuery(
    long Id) : IRequest<PositionVm>;