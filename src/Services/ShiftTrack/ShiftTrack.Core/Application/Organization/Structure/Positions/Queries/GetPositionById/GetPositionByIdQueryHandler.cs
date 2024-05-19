using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Queries.GetPositionById
{
    public class GetPositionByIdQueryHandler : IRequestHandler<GetPositionByIdQuery, PositionVM>
    {
        private readonly IMapper _mapper;
        private readonly IPositionService _positionService;

        public GetPositionByIdQueryHandler(
            IMapper mapper,
            IPositionService positionService)
        {
            _mapper = mapper;
            _positionService = positionService;
        }

        public async Task<PositionVM> Handle(GetPositionByIdQuery request, CancellationToken cancellationToken)
        {
            var position = await _positionService.GetById(request.Id, cancellationToken);

            return _mapper.Map<PositionVM>(position);
        }
    }
}
