namespace Zeta.ECS;

/// <summary>
/// A representation of a concept, 
/// including the default parameters of any components such and object would have.
/// </summary>
public class Prototype
{
    /// <summary>
    /// Internal reference ID, must be unique to this prototype.
    /// </summary>
    public readonly string ID;

    /// <summary>
    /// Plaintext name of the Prototype. Used when prototype is displayed
    /// </summary>
    public readonly string Name;

    private Dictionary<Type, Component> _components = new();

    /// <summary>
    /// A representation of a concept, 
    /// including the default parameters of any components such and object would have.
    /// </summary>
    /// <param name="id">Internal reference ID, must be unique to this prototype.</param>
    /// <param name="name">Plaintext name of the prototype. Used when prototype is displayed</param>
    public Prototype(string id, string name)
    {
        ID = id;
        Name = name;
    }

    public void AddComponent<T>(T component) where T : Component => _components.Add(typeof(T), component);
    public void AddComponent(Type componentType, Component componentData) => _components.Add(componentType, componentData);
    public bool HasComponent<T>() where T : Component => _components.ContainsKey(typeof(T));
    public bool RemoveComponent<T>() where T : Component => _components.Remove(typeof(T));
    public bool TryGetComponent<T>(out T? component) where T : Component
    {
        if (_components.TryGetValue(typeof(T), out Component? comp))
        {
            component = comp as T;
            return component != null;
        }
        component = null;
        return false;
    }
}

public static class PrototypeRegistry
{
    private static Dictionary<string, Prototype> _prototypes = new();

    public static void Add(Prototype prototype) => _prototypes.Add(prototype.ID, prototype);
    public static bool TryGet(string key, out Prototype? prototype) => _prototypes.TryGetValue(key, out prototype);

    /// <summary>
    /// Attempts to retrieve a list of prototypes with a given component type
    /// </summary>
    /// <typeparam name="T">The type of the component</typeparam>
    /// <param name="prototypes">A list containing prototypes with the given component types</param>
    /// <returns>True if any prototypes with the given component type were found, otherwise false</returns>
    public static bool TryGetAllWithComponent<T>(out List<Prototype> prototypes) where T : Component
    {
        prototypes = new List<Prototype>();

        foreach (var key in _prototypes.Keys)
        {
            var proto = _prototypes[key];
            if (proto.HasComponent<T>()) prototypes.Add(proto);
        }

        return prototypes.Count > 0;
    }
}