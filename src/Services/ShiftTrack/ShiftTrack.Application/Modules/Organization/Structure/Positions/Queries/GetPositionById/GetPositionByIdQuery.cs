using ShiftTrack.Application.Modules.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.Queries.GetPositionById;

public record GetPositionByIdQuery(
    long Id) : IRequest<PositionVm>;