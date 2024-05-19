using Kernel.Exceptions;
using MediatR;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.DeletePosition
{
    public class DeletePositionCommandHandler : IRequestHandler<DeletePositionCommand>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public DeletePositionCommandHandler(
            IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<MediatR.Unit> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
        {
            var position = await _applicationDbContext.Positions
                .FindAsync(request.Id);

            if (position is null)
            {
                throw new EntityNotFoundException(typeof(Position), request.Id);
            }

            _applicationDbContext.Positions.Remove(position);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return MediatR.Unit.Value;
        }
    }
}
