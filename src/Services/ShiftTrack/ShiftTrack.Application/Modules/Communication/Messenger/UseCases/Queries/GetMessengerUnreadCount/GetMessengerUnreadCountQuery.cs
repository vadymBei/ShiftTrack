using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.GetMessengerUnreadCount;

public record GetMessengerUnreadCountQuery : IRequest<MessengerUnreadCountVm>;
