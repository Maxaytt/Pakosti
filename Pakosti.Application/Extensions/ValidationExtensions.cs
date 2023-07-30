using System.Linq.Expressions;
using FluentValidation;

namespace Pakosti.Application.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilder<T, string> Email<T>(this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder
        .NotEmpty().WithMessage("Email is required")
        .EmailAddress().WithMessage("Invalid email address")
        .MaximumLength(50).WithMessage("Email must not exceed 50 characters");
    
    public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder
        .NotEmpty().WithMessage("Password is required")
        .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
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
        .MaximumLength(64).WithMessage("Firstname must not exceed 64 characters");
    
    public static IRuleBuilder<T, string> LastName<T>(this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder
        .NotEmpty().WithMessage("Last name is required")
        .MaximumLength(64).WithMessage("Username must not exceed 64 characters");
    
    public static IRuleBuilder<T, string> Username<T>(this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder
        .NotEmpty().WithMessage("Username is required")
        .MinimumLength(5).WithMessage("Username must contain at least 5 characters")
        .MaximumLength(25).WithMessage("Username must not exceed 25 characters");
    
    public static IRuleBuilder<T, DateTime> BirthDate<T>(this IRuleBuilder<T, DateTime> ruleBuilder) => ruleBuilder
        .NotEmpty()
        .LessThanOrEqualTo(DateTime.Today.AddYears(-18))
        .WithMessage("You must be at least 18 years old");
}
