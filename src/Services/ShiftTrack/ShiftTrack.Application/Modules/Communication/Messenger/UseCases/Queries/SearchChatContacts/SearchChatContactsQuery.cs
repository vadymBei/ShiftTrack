using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.SearchChatContacts;

public record SearchChatContactsQuery(
    string Search) : IRequest<List<ChatContactVm>>;
