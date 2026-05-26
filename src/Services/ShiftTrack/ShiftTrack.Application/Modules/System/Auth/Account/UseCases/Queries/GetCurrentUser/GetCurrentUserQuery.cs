using ShiftTrack.Application.Modules.System.Auth.Account.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Queries.GetCurrentUser;

public record GetCurrentUserQuery() : IRequest<CurrentUserVm>;