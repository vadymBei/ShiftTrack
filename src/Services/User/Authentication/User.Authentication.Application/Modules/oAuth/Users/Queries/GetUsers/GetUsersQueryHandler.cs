using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Modules.oAuth.Users.Queries.GetUsers;

public class GetUsersQueryHandler(
    IMapper mapper,
    IUserService userService) : IRequestHandler<GetUsersQuery, IEnumerable<UserVm>>
{
    public async Task<IEnumerable<UserVm>> Handle(GetUsersQuery request, CancellationToken cancellationToken = default)
    {
        var users = await userService.GetUsers(cancellationToken);
        
        return mapper.Map<IEnumerable<UserVm>>(users);
    }
}