using FluentValidation;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Extensions.ValidationExtensions;

namespace Pakosti.Application.Validators;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator(IConfiguration configuration)
    {
        RuleFor(password => password).Password(configuration);
    }
}