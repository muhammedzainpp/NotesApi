using FluentValidation;

namespace Notes.Application.UserProfiles.Commands.Validations;

public class SaveUserProfileCommandValidator : AbstractValidator<SaveUserProfileCommand>
{
    public SaveUserProfileCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.AppUserId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
    }
}
