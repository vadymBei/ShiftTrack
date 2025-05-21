using AutoMapper;
using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Core.Application.Common.Interfaces;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Users.Queries;

public class GetAllUsersQueryHandler(
    IMapper mapper,
    IUserService userService) : IRequestHandler<GetAllUsersQuery, IEnumerable<UserVM>>
{
    public async Task<IEnumerable<UserVM>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken = default)
    {
        var users = await userService.GetUsers(cancellationToken);
        
        return mapper.Map<IEnumerable<UserVM>>(users);
    }
}