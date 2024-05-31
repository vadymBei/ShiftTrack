using AutoMapper;
using MediatR;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserVM>
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UpdateUserCommandHandler(
            IMapper mapper, 
            IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<UserVM> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.UpdateUser(
                new UserToUpdateDto(
                    request.Id,
                    request.Email,
                    request.PhoneNumber));

            return _mapper.Map<UserVM>(user);
        }
    }
}
