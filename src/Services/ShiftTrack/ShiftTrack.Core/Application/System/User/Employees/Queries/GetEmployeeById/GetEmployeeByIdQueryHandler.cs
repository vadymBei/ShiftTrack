using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.Employees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeVM>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public GetEmployeeByIdQueryHandler(
            IMapper mapper,
            IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }

        public async Task<EmployeeVM> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await _employeeService.GetById(request.Id, cancellationToken);

            return _mapper.Map<EmployeeVM>(employee);
        }
    }
}
