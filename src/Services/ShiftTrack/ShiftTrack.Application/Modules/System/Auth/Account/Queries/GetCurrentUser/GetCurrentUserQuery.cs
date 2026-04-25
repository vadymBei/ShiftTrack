using ShiftTrack.Application.Modules.System.Auth.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Account.Queries.GetCurrentUser;

public record GetCurrentUserQuery() : IRequest<CurrentUserVm>;