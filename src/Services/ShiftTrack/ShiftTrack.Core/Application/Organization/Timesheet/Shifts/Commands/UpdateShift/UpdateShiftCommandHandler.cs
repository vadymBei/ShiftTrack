using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.UpdateShift;

public class UpdateShiftCommandHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<UpdateShiftCommand, ShiftVM>
{
    public async Task<ShiftVM> Handle(UpdateShiftCommand request, CancellationToken cancellationToken)
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

        return mapper.Map<ShiftVM>(shift);
    }
}