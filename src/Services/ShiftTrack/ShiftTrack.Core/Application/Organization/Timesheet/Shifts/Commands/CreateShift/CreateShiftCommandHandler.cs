using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.CreateShift;

public class CreateShiftCommandHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<CreateShiftCommand, ShiftVM>
{
    public async Task<ShiftVM> Handle(CreateShiftCommand request, CancellationToken cancellationToken)
    {
        var shift = new Shift()
        {
            Code = request.Code,
            Description = request.Description,
            Type = request.Type,
            Color = request.Color
        };

        await applicationDbContext.Shifts.AddAsync(shift, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<ShiftVM>(shift);
    }
}