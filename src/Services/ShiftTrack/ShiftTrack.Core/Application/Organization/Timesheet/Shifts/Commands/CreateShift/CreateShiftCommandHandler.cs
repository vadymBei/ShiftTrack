using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.CreateShift
{
    public class CreateShiftCommandHandler : IRequestHandler<CreateShiftCommand, ShiftVM>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _applicationDbContext;

        public CreateShiftCommandHandler(
            IMapper mapper,
            IApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ShiftVM> Handle(CreateShiftCommand request, CancellationToken cancellationToken)
        {
            var shift = new Shift()
            {
                Code = request.Code,
                Dercription = request.Dercription,
                Type = request.Type,
                Color = request.Color
            };

            await _applicationDbContext.Shifts.AddAsync(shift, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ShiftVM>(shift);
        }
    }
}
