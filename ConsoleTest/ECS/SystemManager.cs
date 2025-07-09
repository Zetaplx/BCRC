namespace Zeta.ECS;

using System.Reflection;

public static class SystemManager
{
    private static Dictionary<Type, ProtoSystem> _systems = new();

    private static void Initialize(Assembly assembly)
    {
        _systems = new Dictionary<Type, ProtoSystem>();

        var systemTypes = assembly.GetTypes().Where(t => t.IsAssignableFrom(typeof(ProtoSystem)));
        foreach (var systemType in systemTypes)
        {
            var system = Activator.CreateInstance(systemType) as ProtoSystem;
            if (system == null)
                throw new Exception($"Unable to create system instance of type {systemType}, cannot cast to ProtoSystem");

            _systems.Add(systemType, system);
        }
    }

    public static void Start()
    {
        foreach (var system in _systems.Values)
        {
            system.Start();
        }
    }

    public static bool TryGetSystem<T>(out T? system) where T : ProtoSystem
    {
        system = null;
        if (!_systems.TryGetValue(typeof(T), out var value))
            return false;

        system = value as T;
        return system != null;
    }
}