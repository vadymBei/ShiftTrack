using MediatR;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.Employees.Queries.GetCurrentUser;

public record GetCurrentUserQuery() : IRequest<CurrentUserVM>;