using FluentValidation;

namespace User.Authentication.Application.Features.oAuth.Roles.Commands.CreateRole;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Data.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(128)
            .WithMessage("Maximum length must be 128 symbols");
    }
}