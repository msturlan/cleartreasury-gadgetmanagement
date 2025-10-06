using System.Reflection;

namespace ClearTreasury.GadgetManagement.Api.Extensions;

public static class TypeExtensions
{
    public static string[] GetNonEmptyStringConsts(this Type type)
    {
        var results = new List<string>();

        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
        {
            if (field.IsLiteral && !field.IsInitOnly)
            {
                if (field.GetValue(null) is string value && !String.IsNullOrWhiteSpace(value))
                {
                    results.Add(value);
                }
            }
        }

        return [.. results];
    }
}
