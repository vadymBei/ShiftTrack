using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Queries.GetPositionById
{
    public class GetPositionByIdQuery : IRequest<PositionVM>
    {
        public long Id { get; set; }
    }
}
