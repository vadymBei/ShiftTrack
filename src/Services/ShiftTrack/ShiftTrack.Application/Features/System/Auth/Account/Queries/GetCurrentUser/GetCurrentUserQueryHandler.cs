using AutoMapper;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.System.Auth.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.Auth.Account.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler(
    IMapper mapper,
    ICurrentUserService currentUserService) : IRequestHandler<GetCurrentUserQuery, CurrentUserVm>
{
    public async Task<CurrentUserVm> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var currentUser = await currentUserService.GetCurrentUser(cancellationToken);

        return mapper.Map<CurrentUserVm>(currentUser);
    }
}