using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Features.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Features.oAuth.Users.Queries.GetUsers;

public record GetUsersQuery : IRequest<IEnumerable<UserVm>>;