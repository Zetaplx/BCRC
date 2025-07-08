using Zeta.ECS;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class Program
{
    public static void Main(string[] args)
    {
        string yaml = File.ReadAllText("C:/Users/kylgr/Desktop/BCRC/ConsoleTest/Test Data/test_data.yml");


        var rawData = new DeserializerBuilder()
            .WithNamingConvention(HyphenatedNamingConvention.Instance)
            .Build()
            .Deserialize<List<Dictionary<string, object>>>(yaml);

        Console.WriteLine("Reading Raw Data:");
        foreach (var data in rawData)
        {
            string id = data["id"].ToString() ?? "invalid";
            string name = data["name"].ToString() ?? id;

            Console.WriteLine($"{id} - {name}");
            if (data.TryGetValue("components", out var componentsObject) && componentsObject is List<object> componentObjects)
            {
                foreach (var compObj in componentObjects)
                {
                    if (compObj is Dictionary<object, object> kvp)
                    {
                        Console.WriteLine($"\t{kvp.Keys.First()}");
                        if (kvp.Values.First() is Dictionary<object, object> compProps)
                        {
                            foreach (var compProp in compProps)
                            {
                                Console.WriteLine($"\t\t{compProp.Key}:{compProp.Value}");
                            }
                        }
                        
                    }
                    
                }
            }
        }
    }
}