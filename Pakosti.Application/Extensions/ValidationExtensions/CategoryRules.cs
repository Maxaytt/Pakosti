using FluentValidation;
using Microsoft.Extensions.Configuration;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Extensions.ValidationExtensions;

public static class CategoryRules
{
    public static IRuleBuilder<T, string?> CategoryName<T>(this IRuleBuilder<T, string?> ruleBuilder,
        IConfiguration configuration)
    {
        var lengths = configuration.GetSection($"Settings:{nameof(Category)}:NameLengths").Get<int[]>()!;
        var (minLength, maxLength) = (lengths[0], lengths[1]);
        
        return ruleBuilder
            .MinimumLength(minLength).WithMessage($"Name must contain at least {minLength} characters")
            .MaximumLength(maxLength).WithMessage($"Name must not exceed {maxLength} characters");
    }
}