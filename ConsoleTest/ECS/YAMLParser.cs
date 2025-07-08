using YamlDotNet.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Serialization.NamingConventions;
using Zeta.ECS;
using System.Reflection;
using System.Linq;

public class PrototypeParser
{
    public static void Load(string yaml)
    {
        var rawData = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build()
            .Deserialize<List<Dictionary<string, object>>>(yaml);

        var rawPrototypes = new List<RawPrototype>();

        foreach (var entry in rawData)
        {
            string id = entry["id"].ToString() ?? "invalid";
            string name = entry["name"].ToString() ?? id;

            var prototype = new RawPrototype(id, name);

            if (entry.TryGetValue("components", out var componentsObject) && componentsObject is List<object> componentObjects)
            {
                foreach (var compObj in componentObjects.OfType<Dictionary<object, object>>())
                {
                    var key = compObj.Keys.First().ToString() ?? "invalid";
                    var data = compObj[compObj.Keys.First()] as Dictionary<object, object>;
                    if (data == null) continue;

                    var normalized = new Dictionary<string, object>();
                    foreach (var dataKey in data.Keys)
                    {
                        normalized.Add(dataKey.ToString() ?? "invalid", data[dataKey]);
                    }

                    prototype.Components.Add(new RawComponent(key, normalized));
                }
            }

            rawPrototypes.Add(prototype);
        }

        foreach (var rawProto in rawPrototypes)
        {
            PrototypeRegistry.Add(rawProto.Parse());
        }
    }
}

public class RawPrototype
{
    public string ID { get; set; }
    public string Name { get; set; }

    public List<RawComponent> Components { get; set; } = new();

    public RawPrototype(string id, string name)
    {
        ID = id;
        Name = name;
    }

    public Prototype Parse()
    {
        Prototype prototype = new Prototype(ID, Name);

        foreach (var comp in Components)
        {
            comp.Parse(out var type, out var data);
            if (type != null && data != null) prototype.AddComponent(type, data);
        }

        return prototype;
    }
}

public class RawComponent
{
    public string Key { get; set; }
    public Dictionary<string, object> Data { get; set; }

    public RawComponent(string key, Dictionary<string, object> data)
    {
        Key = key;
        Data = data;
    }

    public void Parse(out Type? componentType, out Component? componentData)
    {
        if (ComponentRegistry.TryGetComponentType(Key, out componentType))
        {
            var comp = Activator.CreateInstance(componentType);
            foreach (var prop in componentType.GetProperties())
            {
                var key = prop.GetCustomAttribute<ComponentPropertyAttribute>()?.Key ?? CamelToKebab(prop.Name);
            }
            componentData = null;
            return;
        }
        componentData = null;
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
}
