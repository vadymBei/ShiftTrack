using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Queries.GetUnitById
{
    public class GetUnitByIdQueryHandler : IRequestHandler<GetUnitByIdQuery, UnitVM>
    {
        private readonly IMapper _mapper;
        private readonly IUnitService _unitService;

        public GetUnitByIdQueryHandler(
            IMapper mapper,
            IUnitService unitService)
        {
            _mapper = mapper;
            _unitService = unitService;
        }

        public async Task<UnitVM> Handle(GetUnitByIdQuery request, CancellationToken cancellationToken)
        {
            var unit = await _unitService.GetById(request.Id, cancellationToken);

            return _mapper.Map<UnitVM>(unit);
        }
    }
}
