using FluentValidation;
using Microsoft.Extensions.Configuration;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Extensions.ValidationExtensions;

public static class ReviewRules
{
    public static IRuleBuilder<T, string?> ReviewHeader<T>(this IRuleBuilder<T, string?> ruleBuilder,
        IConfiguration configuration)
    {
        var lengths = configuration.GetSection($"Settings:{nameof(Review)}:HeaderLengths").Get<int[]>()!;
        var (minLength, maxLength) = (lengths[0], lengths[1]);
        return ruleBuilder
            .MinimumLength(minLength).WithMessage($"Header must contain at least {minLength} characters")
            .MaximumLength(maxLength).WithMessage($"Header must not exceed {maxLength} characters");
    }

    public static IRuleBuilder<T, string?> ReviewBody<T>(this IRuleBuilder<T, string?> ruleBuilder,
        IConfiguration configuration)
    {
        var lengths = configuration.GetSection($"Settings:{nameof(Review)}:BodyLengths").Get<int[]>()!;
        var (minLength, maxLength) = (lengths[0], lengths[1]);
        return ruleBuilder
            .MinimumLength(minLength).WithMessage($"Body must contain at least {minLength} characters")
            .MaximumLength(maxLength).WithMessage($"Body must not exceed {maxLength} characters");
    }
}