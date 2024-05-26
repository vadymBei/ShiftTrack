using AutoMapper;
using MediatR;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserVM>
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public CreateUserCommandHandler(
            IMapper mapper,
            IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<UserVM> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _userService
                .CreateUser(
                    new UserToCreateDto
                    (
                        request.Email,
                        request.FullName,
                        request.Password,
                        request.ProfileId
                    ));

            return _mapper.Map<UserVM>(result);
        }
    }
}
