using System.Text.RegularExpressions;

namespace Pakosti.Api.Services;

public class KebabTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value is null) return null;

        var transformedValue = Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2");
        return transformedValue.ToLower();
    }
}