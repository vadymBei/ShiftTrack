using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Queries.GetUnitById;

public record GetUnitByIdQuery(
    long Id) : IRequest<UnitVM>;