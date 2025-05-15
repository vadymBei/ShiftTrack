using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Employees.Queries.GetCurrentUser;

public record GetCurrentUserQuery() : IRequest<CurrentUserVM>;