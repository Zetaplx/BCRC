namespace Zeta.ECS;

public class Entity
{
    public EntityUID _uid { get; private set; } = new EntityUID();
    private Dictionary<Type, Component> _components { get; set; } = new Dictionary<Type, Component>();

    public void AddComponent<T>(T component) where T : Component => _components.Add(typeof(T), component);
    public bool HasComponent<T>() where T : Component => _components.ContainsKey(typeof(T));
    public bool RemoveComponent<T>() where T : Component => _components.Remove(typeof(T));
}

public struct EntityUID
{
    private static int _nextId = 0;
    private int _id;

    public EntityUID()
    {
        _id = _nextId++;
    }

    public EntityUID(int id)
    {
        _id = id;
    }

    #region Equivalence
    public override int GetHashCode()
    {
        return _id;
    }
    public override bool Equals(object? obj)
    {
        return obj is EntityUID other && _id == other._id;
    }
    public static bool operator ==(EntityUID left, EntityUID right)
    {
        return left.Equals(right);
    }
    public static bool operator !=(EntityUID left, EntityUID right)
    {
        return !left.Equals(right);
    }
    #endregion
}