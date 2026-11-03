using FluentValidation;
using ShiftTrack.Application.Modules.Communication.Messenger.Constants;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.SearchChatContacts;

public class SearchChatContactsQueryValidator : AbstractValidator<SearchChatContactsQuery>
{
    public SearchChatContactsQueryValidator()
    {
        RuleFor(x => x.Search)
            .NotEmpty()
            .Must(x => x?.Trim().Length >= 2)
            .WithMessage("Search must be at least 2 characters long.")
            .MaximumLength(100);
    }
}
