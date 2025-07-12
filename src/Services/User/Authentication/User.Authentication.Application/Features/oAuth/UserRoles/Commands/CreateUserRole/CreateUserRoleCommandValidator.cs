using FluentValidation;

namespace User.Authentication.Application.Features.oAuth.UserRoles.Commands.CreateUserRole;

public class CreateUserRoleCommandValidator : AbstractValidator<CreateUserRoleCommand>
{
    public CreateUserRoleCommandValidator()
    {
        RuleFor(x => x.Data.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.")
            .Must(BeValidGuid)
            .WithMessage("UserId must be a valid GUID.");

        RuleFor(x => x.Data.RoleId)
            .NotEmpty()
            .WithMessage("RoleId is required.")
            .Must(BeValidGuid)
            .WithMessage("RoleId must be a valid GUID.");
    }

    private bool BeValidGuid(string guid)
    {
        return Guid.TryParse(guid, out _);
    }
}