using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.Employees.Queries.GetEmployees
{
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, IEnumerable<EmployeeVM>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _applicationDbContext;

        public GetEmployeesQueryHandler(
            IMapper mapper,
            IApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<EmployeeVM>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employeeQuery = _applicationDbContext.Employees
                .Include(x => x.Department)
                .Include(x => x.Position)
                .AsQueryable();

            //if (request.UnitId is not null)
            //{
            //    employeeQuery = employeeQuery
            //        .Where(x => x.Department.UnitId == request.UnitId);
            //}

            if (!string.IsNullOrWhiteSpace(request.SearchPattern))
            {
                employeeQuery = employeeQuery
                    .Where(x => EF.Functions.Like(
                        x.Surname.ToLower() + " " + x.Name.ToLower() + " " + x.Patronymic.ToLower(),
                        $"%{request.SearchPattern.ToLower()}%"));
            }

            var employees = await employeeQuery
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<EmployeeVM>>(employees);
        }
    }
}
