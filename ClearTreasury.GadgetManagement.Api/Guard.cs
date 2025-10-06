using System.Runtime.CompilerServices;

namespace ClearTreasury.GadgetManagement.Api;

public static class Guard
{
    public static string NotNullOrWhitespace(string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (String.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                $"Value should not be a null, an empty or a white space only string",
                paramName ?? nameof(value));
        }

        return value;
    }
}
