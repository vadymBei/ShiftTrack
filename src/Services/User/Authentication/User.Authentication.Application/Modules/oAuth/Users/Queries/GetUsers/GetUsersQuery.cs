using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Application.Modules.oAuth.Common.ViewModels;

namespace User.Authentication.Application.Modules.oAuth.Users.Queries.GetUsers;

public record GetUsersQuery : IRequest<IEnumerable<UserVm>>;