namespace Zeta.ECS;

public abstract class Component
{
    public EntityUID ParentUID { get; private set; }

    public Component(EntityUID parentUID)
    {
        ParentUID = parentUID;
    }
}

public static class ComponentRegistry
{
    private static Dictionary<string, Type> _components = new();
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
        }
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