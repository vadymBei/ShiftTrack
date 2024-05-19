using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using UnitEntity = ShiftTrack.Core.Domain.Organization.Structure.Entities.Unit;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Commands.CreateUnit
{
    public class CreateUnitCommandHandler : IRequestHandler<CreateUnitCommand, UnitVM>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _dbContext;

        public CreateUnitCommandHandler(
            IMapper mapper,
            IApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<UnitVM> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
        {
            var unit = new UnitEntity()
            {
                Name = request.Name,
                Description = request.Description,
                Code = request.Code
            };

            await _dbContext.Units.AddAsync(unit, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UnitVM>(unit);
        }
    }
}
