using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.Employees.Queries.GetCurrentUser
{
    internal class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, CurrentUserVM>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public GetCurrentUserQueryHandler(
            IMapper mapper, 
            IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }

        public async Task<CurrentUserVM> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _employeeService.GetCurrentUser(cancellationToken);

            return _mapper.Map<CurrentUserVM>(currentUser);
        }
    }
}
