namespace Chirp.Infrastructure;

public class AuthorValidator : AbstractValidator<AuthorDTO>
{
    public AuthorValidator()
    {
        RuleFor(a => a.Name).NotNull().WithMessage("Name should not be null.")
                            .NotEmpty().WithMessage("Name should not be empty.")
                            .Length(0, 50).WithMessage("Name should have 50 characters at most.");
    }
}