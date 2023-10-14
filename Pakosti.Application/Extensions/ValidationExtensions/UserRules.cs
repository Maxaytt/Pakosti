using System.Linq.Expressions;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Pakosti.Application.Extensions.ValidationExtensions;

public static class UserRules
{
    public static IRuleBuilder<T, string?> Email<T>(this IRuleBuilder<T, string?> ruleBuilder, 
        IConfiguration configuration)
    {
        var maxLength = configuration.GetSection("Settings:User:EmailMaxLength").Get<int>();
        return ruleBuilder
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address")
            .MaximumLength(maxLength)
            .WithMessage($"Email must not exceed {maxLength} characters");
    }

    public static IRuleBuilder<T, string?> Password<T>(this IRuleBuilder<T, string?> ruleBuilder,
        IConfiguration configuration)
    {
        var minLength = configuration.GetSection("Settings:User:PasswordMinLength").Get<int>();
        return ruleBuilder
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(minLength)
            .WithMessage($"Password must be at least {minLength} characters long")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit")
            .Matches("[!@#$%\\^&*()\\[\\]{};':\",.<>\\/\\-=_+]")
            .WithMessage("Password must contain at least one special character");
    }

    public static IRuleBuilder<T, string?> PasswordConfirm<T>(this IRuleBuilder<T, string?> ruleBuilder, 
        Expression<Func<T, string?>> passwordProperty) => ruleBuilder
            .NotEmpty().WithMessage("Confirm password is required")
            .Equal(passwordProperty).WithMessage("Passwords do not match");

    public static IRuleBuilder<T, string?> FirstName<T>(this IRuleBuilder<T, string?> ruleBuilder,
        IConfiguration configuration)
    {
        var maxLength = configuration.GetSection("Settings:User:FirstNameMaxLength").Get<int>();
        return ruleBuilder
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(maxLength)
            .WithMessage($"Firstname must not exceed {maxLength} characters");
    }

    public static IRuleBuilder<T, string?> LastName<T>(this IRuleBuilder<T, string?> ruleBuilder,
        IConfiguration configuration)
    {
        var maxLength = configuration.GetSection("Settings:User:LastNameMaxLength").Get<int>();
        return ruleBuilder
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(maxLength).WithMessage($"Username must not exceed {maxLength} characters");
    }

    public static IRuleBuilder<T, string?> Username<T>(this IRuleBuilder<T, string?> ruleBuilder,
        IConfiguration configuration)
    {
        var lengths = configuration.GetSection("Settings:User:UsernameLengths").Get<int[]>()!;
        var (minLength, maxLength) = (lengths[0], lengths[1]);
        return ruleBuilder
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(minLength)
            .WithMessage($"Username must contain at least {minLength} characters")
            .MaximumLength(maxLength)
            .WithMessage($"Username must not exceed {maxLength} characters");
    }

    public static IRuleBuilder<T, DateTime?> BirthDate<T>(this IRuleBuilder<T, DateTime?> ruleBuilder) => ruleBuilder
        .LessThanOrEqualTo(DateTime.Today.AddYears(-18))
        .WithMessage("You must be at least 18 years old");
    
    public static IRuleBuilder<T, DateTime> BirthDate<T>(this IRuleBuilder<T, DateTime> ruleBuilder) => ruleBuilder
        .NotEmpty()
        .LessThanOrEqualTo(DateTime.Today.AddYears(-18))
        .WithMessage("You must be at least 18 years old");
}