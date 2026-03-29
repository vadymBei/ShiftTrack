using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.System.Auth.Common.ViewModels;
using ShiftTrack.Domain.Features.System.Auth.Models;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.Auth.Account.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler(
    IMapper mapper,
    ICurrentUserService currentUserService) : IRequestHandler<GetCurrentUserQuery, CurrentUserVm>
{
    public async Task<CurrentUserVm> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var currentUser = new CurrentUser
        {
            Employee = currentUserService.Employee,
            Roles = currentUserService.Roles
        };

        return await Task.FromResult(mapper.Map<CurrentUserVm>(currentUser));
    }
}