using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.UpdatePosition
{
    public class UpdatePositionCommandHandler : IRequestHandler<UpdatePositionCommand, PositionVM>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _applicationDbContext;

        public UpdatePositionCommandHandler(
            IMapper mapper,
            IApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<PositionVM> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
        {
            var position = await _applicationDbContext.Positions
                .FindAsync(request.Id);

            if (position is null)
            {
                throw new EntityNotFoundException(typeof(Position), request.Id);
            }

            position.Name = request.Name;
            position.Description = request.Description;

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PositionVM>(position);
        }
    }
}
