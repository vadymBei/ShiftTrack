using AutoMapper;
using MediatR;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, TokenVM>
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public ChangePasswordCommandHandler(
            IMapper mapper,
            IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<TokenVM> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var token = await _userService
                .ChangePassword(request.Data, cancellationToken);

            return _mapper.Map<TokenVM>(token);
        }
    }
}
