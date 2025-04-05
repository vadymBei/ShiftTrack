using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.Employees.Queries.GetCurrentUser;

internal class GetCurrentUserQueryHandler(
    IMapper mapper,
    IEmployeeService employeeService) : IRequestHandler<GetCurrentUserQuery, CurrentUserVM>
{
    public async Task<CurrentUserVM> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var currentUser = await employeeService.GetCurrentUser(cancellationToken);

        return mapper.Map<CurrentUserVM>(currentUser);
    }
}