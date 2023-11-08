namespace Chirp.Infrastructure;

public class CheepValidator : AbstractValidator<CheepDTO>
{
    public CheepValidator()
    {
        RuleFor(c => c.Author).NotNull().WithMessage("Author should not be null.")
                              .NotEmpty().WithMessage("Author should not be empty.");

        RuleFor(c => c.Text).NotNull().WithMessage("Text should not be null.")
                            .NotEmpty().WithMessage("Text should not be empty.")
                            .Length(0, 160).WithMessage("Text should have 160 characters at most.");

        RuleFor(c => c.TimeStamp).NotNull().WithMessage("TimeStamp should not be null.")
                                 .NotEmpty().WithMessage("TimeStamp should not be empty.");
    }
}