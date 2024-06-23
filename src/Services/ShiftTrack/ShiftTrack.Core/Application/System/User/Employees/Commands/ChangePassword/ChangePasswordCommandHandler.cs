using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.System.Auth.Common.ViewModels;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Employees.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, TokenVM>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public ChangePasswordCommandHandler(
            IMapper mapper, 
            IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }

        public async Task<TokenVM> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var token = await _employeeService
                .ChangePassword(request.Data, cancellationToken);

            return _mapper.Map<TokenVM>(token);
        }
    }
}
