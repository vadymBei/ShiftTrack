using ShiftTrack.Kernel.CQRS.Interfaces;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Users.Queries;

public record GetAllUsersQuery() : IRequest<IEnumerable<UserVM>>;