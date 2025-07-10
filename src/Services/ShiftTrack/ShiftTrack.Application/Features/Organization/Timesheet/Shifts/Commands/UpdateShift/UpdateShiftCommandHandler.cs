using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Shifts.Commands.UpdateShift;

public class UpdateShiftCommandHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<UpdateShiftCommand, ShiftVm>
{
    public async Task<ShiftVm> Handle(UpdateShiftCommand request, CancellationToken cancellationToken)
    {
        var shift = await applicationDbContext.Shifts
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (shift == null)
        {
            throw new EntityNotFoundException(typeof(Shift), request.Id);
        }

        shift.Code = request.Code;
        shift.Description = request.Description;
        shift.Color = request.Color;
        shift.Type = request.Type;
        shift.StartTime = request.StartTime;
        shift.EndTime = request.EndTime;

        if (request.StartTime.HasValue 
            && request.EndTime.HasValue)
        {
            shift.WorkHours = request.EndTime.Value - request.StartTime.Value;
        }
        else
        {
            shift.WorkHours = null;
        }
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<ShiftVm>(shift);
    }
}