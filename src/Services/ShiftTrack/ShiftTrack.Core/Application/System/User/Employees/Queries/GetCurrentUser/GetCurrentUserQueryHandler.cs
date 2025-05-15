using AutoMapper;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Employees.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler(
    IMapper mapper,
    IEmployeeService employeeService) : IRequestHandler<GetCurrentUserQuery, CurrentUserVM>
{
    public async Task<CurrentUserVM> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var currentUser = await employeeService.GetCurrentUser(cancellationToken);

        return mapper.Map<CurrentUserVM>(currentUser);
    }
}