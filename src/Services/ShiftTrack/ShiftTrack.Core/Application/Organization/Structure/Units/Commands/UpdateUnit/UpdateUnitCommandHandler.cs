using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Commands.UpdateUnit
{
    public class UpdateUnitCommandHandler : IRequestHandler<UpdateUnitCommand, UnitVM>
    {
        private readonly IMapper _mapper;
        private readonly IUnitService _unitService;
        private readonly IApplicationDbContext _dbContext;

        public UpdateUnitCommandHandler(
            IMapper mapper,
            IUnitService unitService,
            IApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _unitService = unitService;
            _dbContext = dbContext;
        }

        public async Task<UnitVM> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
        {
            var unit = await _dbContext.Units
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (unit == null)
                throw new EntityNotFoundException(typeof(Unit), request.Id);

            unit.Name = request.Name;
            unit.Description = request.Description;
            unit.Code = request.Code;

            await _dbContext.SaveChangesAsync(cancellationToken);

            unit = await _unitService.GetById(request.Id, cancellationToken);

            return _mapper.Map<UnitVM>(unit);
        }
    }
}
