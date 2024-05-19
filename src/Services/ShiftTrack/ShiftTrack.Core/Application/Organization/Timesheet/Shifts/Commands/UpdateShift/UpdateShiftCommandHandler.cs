using AutoMapper;
using Kernel.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.UpdateShift
{
    public class UpdateShiftCommandHandler : IRequestHandler<UpdateShiftCommand, ShiftVM>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _applicationDbContext;

        public UpdateShiftCommandHandler(
            IMapper mapper,
            IApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ShiftVM> Handle(UpdateShiftCommand request, CancellationToken cancellationToken)
        {
            var shift = await _applicationDbContext.Shifts
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (shift == null)
            {
                throw new EntityNotFoundException(typeof(Shift), request.Id);
            }

            shift.Code = request.Code;
            shift.Dercription = request.Dercription;
            shift.Color = request.Color;
            shift.Type = request.Type;

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ShiftVM>(shift);
        }
    }
}
