namespace Zeta.ECS;

public abstract class Component { }

public static class ComponentRegistry
{
    private static Dictionary<string, Type> _components = new();
    private static Dictionary<string, (string yaml, string raw)> _properties = new();
    static ComponentRegistry()
    {
        var types = typeof(ComponentRegistry).Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Component)) && !t.IsAbstract);

        foreach (var type in types)
        {
            var attribute = (ComponentKeyAttribute?)Attribute.GetCustomAttribute(type, typeof(ComponentKeyAttribute));
            if (attribute != null)
            {
                _components.Add(attribute.Key, type);
            }
            else
            {
                string key = CamelToKebab(type.Name.Replace("Component", ""));
                _components.Add(key, type);
            }
        }
    }

    private static string CamelToKebab(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Insert a hyphen before each uppercase letter (except the first character),
        // then convert the whole string to lowercase.
        var kebab = System.Text.RegularExpressions.Regex.Replace(
            input,
            "(?<!^)([A-Z])",
            "-$1"
        );
        return kebab.ToLower();
    }

    public static bool TryGetComponentType(string key, out Type? type) => _components.TryGetValue(key, out type);

}

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ComponentKeyAttribute : Attribute
{
    public string Key { get; }

    public ComponentKeyAttribute(string key)
    {
        Key = key;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class ComponentPropertyAttribute : Attribute
{
    public string Key { get; }

    public ComponentPropertyAttribute(string key)
    {
        Key = key;
    }
}