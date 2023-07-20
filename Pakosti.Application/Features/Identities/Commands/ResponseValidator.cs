using FluentValidation;
namespace Pakosti.Application.Features.Identities.Commands;

public class ResponseValidator : AbstractValidator<Authenticate.Response>
{
    public ResponseValidator()
    {
        RuleFor(response => response.Username).NotEmpty().WithMessage("Username is required.");
        RuleFor(response => response.Email).NotEmpty().EmailAddress().WithMessage("Valid email is required.");
        RuleFor(response => response.Token).NotEmpty().WithMessage("Access token is required.");
        RuleFor(response => response.RefreshToken).NotEmpty().WithMessage("Refresh token is required.");
    }

}