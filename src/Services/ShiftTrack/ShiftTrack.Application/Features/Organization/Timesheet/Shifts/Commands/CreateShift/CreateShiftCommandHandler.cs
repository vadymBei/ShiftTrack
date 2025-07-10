using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Shifts.Commands.CreateShift;

public class CreateShiftCommandHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<CreateShiftCommand, ShiftVm>
{
    public async Task<ShiftVm> Handle(CreateShiftCommand request, CancellationToken cancellationToken)
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

        return mapper.Map<ShiftVm>(shift);
    }
}