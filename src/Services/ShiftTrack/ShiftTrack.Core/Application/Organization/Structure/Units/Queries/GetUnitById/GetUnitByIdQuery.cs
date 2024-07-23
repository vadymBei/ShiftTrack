using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Queries.GetUnitById
{
    public record GetUnitByIdQuery(
        long Id) : IRequest<UnitVM>;
}
