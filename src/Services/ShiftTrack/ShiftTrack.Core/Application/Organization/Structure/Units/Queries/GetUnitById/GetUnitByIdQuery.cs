using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Queries.GetUnitById
{
    public class GetUnitByIdQuery : IRequest<UnitVM>
    {
        public long Id { get; set; }
    }
}
