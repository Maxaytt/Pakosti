using System.Linq.Expressions;
using FluentValidation;

namespace Pakosti.Application.Extensions.ValidationExtensions;

public static class UserRules
{
    private const int EmailMaxLength = 50;
    private const int PasswordMinLength = 8;
    private const int FirstNameMaxLength = 64;
    private const int LastNameMaxLength = 64;
    private const int UsernameMinLength = 5;
    private const int UsernameMaxLength = 25;
    public static IRuleBuilder<T, string> Email<T>(this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder
        .NotEmpty().WithMessage("Email is required")
        .EmailAddress().WithMessage("Invalid email address")
        .MaximumLength(EmailMaxLength)
        .WithMessage($"Email must not exceed {EmailMaxLength} characters");
    
    public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder
        .NotEmpty().WithMessage("Password is required")
        .MinimumLength(PasswordMinLength)
        .WithMessage($"Password must be at least {PasswordMinLength} characters long")
        .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
        .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
        .Matches("[0-9]").WithMessage("Password must contain at least one digit")
        .Matches("[!@#$%\\^&*()\\[\\]{};':\",.<>\\/\\-=_+]").WithMessage("Password must contain at least one special character");
    
    public static IRuleBuilder<T, string> PasswordConfirm<T>(this IRuleBuilder<T, string> ruleBuilder, 
        Expression<Func<T, string>> passwordProperty) => ruleBuilder
            .NotEmpty().WithMessage("Confirm password is required")
            .Equal(passwordProperty).WithMessage("Passwords do not match");

    public static IRuleBuilder<T, string> FirstName<T>(this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder
        .NotEmpty().WithMessage("First name is required")
        .MaximumLength(FirstNameMaxLength)
        .WithMessage($"Firstname must not exceed {FirstNameMaxLength} characters");
    
    public static IRuleBuilder<T, string> LastName<T>(this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder
        .NotEmpty().WithMessage("Last name is required")
        .MaximumLength(LastNameMaxLength).WithMessage($"Username must not exceed {LastNameMaxLength} characters");
    
    public static IRuleBuilder<T, string> Username<T>(this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder
        .NotEmpty().WithMessage("Username is required")
        .MinimumLength(UsernameMinLength).WithMessage($"Username must contain at least {UsernameMinLength} characters")
        .MaximumLength(UsernameMaxLength).WithMessage($"Username must not exceed {UsernameMaxLength} characters");
    
    public static IRuleBuilder<T, DateTime> BirthDate<T>(this IRuleBuilder<T, DateTime> ruleBuilder) => ruleBuilder
        .NotEmpty()
        .LessThanOrEqualTo(DateTime.Today.AddYears(-18))
        .WithMessage("You must be at least 18 years old");
}