using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.CreateShift;

public class CreateShiftCommandHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<CreateShiftCommand, ShiftVM>
{
    public async Task<ShiftVM> Handle(CreateShiftCommand request, CancellationToken cancellationToken)
    {
        var isShiftExist = await applicationDbContext.Shifts
            .AsTracking()
            .AnyAsync(x => x.Code == request.Code, cancellationToken);

        if (isShiftExist)
        {
            throw new Exception("Shift already exist.");
        }
        
        var shift = new Shift()
        {
            Code = request.Code,
            Description = request.Description,
            Type = request.Type,
            Color = request.Color
        };

        if (request.StartTime.HasValue 
            && request.EndTime.HasValue)
        {
            shift.StartTime = request.StartTime;
            shift.EndTime = request.EndTime;
            shift.WorkHours = request.EndTime.Value - request.StartTime.Value;
        }
        
        await applicationDbContext.Shifts.AddAsync(shift, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<ShiftVM>(shift);
    }
}