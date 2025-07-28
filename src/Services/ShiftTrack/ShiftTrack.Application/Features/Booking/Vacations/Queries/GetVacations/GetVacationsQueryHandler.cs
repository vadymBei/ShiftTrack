using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Common.ViewModels;
using ShiftTrack.Domain.Features.Booking.Vacations.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Queries.GetVacations;

public class GetVacationsQueryHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext) :IRequestHandler<GetVacationsQuery, IEnumerable<VacationVm>>
{
    public async Task<IEnumerable<VacationVm>> Handle(GetVacationsQuery request, CancellationToken cancellationToken = default)
    {
        var vacationsQuery = applicationDbContext.Vacations
            .AsNoTracking()
            .Include(x => x.Employee)
            .Where(x => x.Employee.Department.UnitId == request.UnitId
                        && x.Employee.DepartmentId == request.DepartmentId);

        if (request.StartDate is not null 
            && request.EndDate is not null)
        {
            vacationsQuery = vacationsQuery.Where(x => x.StartDate >= request.StartDate
                                                       && x.StartDate <= request.EndDate);
        }
        
        if(request.Status is not VacationStatus.None)
        {
            vacationsQuery = vacationsQuery.Where(x => x.Status == request.Status);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchPattern))
        {
            vacationsQuery = vacationsQuery
                .Where(x => EF.Functions.Like(
                    x.Employee.Surname.ToLower() + " " + x.Employee.Name.ToLower() + " " + x.Employee.Patronymic.ToLower(),
                    $"%{request.SearchPattern.ToLower()}%"));
        }

        var vacations = await vacationsQuery
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
        
        return mapper.Map<IEnumerable<VacationVm>>(vacations);
    }
}