using ShiftTrack.Application.Features.System.Auth.Common.ViewModels;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.Auth.Account.Queries.GetCurrentUser;

public record GetCurrentUserQuery() : IRequest<CurrentUserVm>;