namespace Chirp.Infrastructure;

public class FollowValidator : AbstractValidator<FollowDTO>
{
    public FollowValidator()
    {
        RuleFor(f => f.Author).NotNull().WithMessage("Auhtor should not be null.")
                              .NotEmpty().WithMessage("Author should not be empty.");
    }
}