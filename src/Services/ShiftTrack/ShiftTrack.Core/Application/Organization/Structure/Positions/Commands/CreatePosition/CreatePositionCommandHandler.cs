using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.CreatePosition
{
    internal class CreatePositionCommandHandler : IRequestHandler<CreatePositionCommand, PositionVM>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _applicationDbContext;

        public CreatePositionCommandHandler(
            IMapper mapper,
            IApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<PositionVM> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
        {
            var position = new Position()
            {
                Name = request.Name,
                Description = request.Description
            };

            _applicationDbContext.Positions.Add(position);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PositionVM>(position);
        }
    }
}
