using Zeta.ECS;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class Program
{
    public static void Main(string[] args)
    {
        string yaml = File.ReadAllText("C:/Users/kylgr/Desktop/Projects/BCRC/ConsoleTest/Test Data/test_data.yml");


        ParseTest("C:/Users/kylgr/Desktop/Projects/BCRC/ConsoleTest/Test Data/test_data.yml");
    }

    public static void ParseTest(string yamlFilePath)
    {
        Console.WriteLine("Loading test YAML...");

        if (!File.Exists(yamlFilePath))
        {
            Console.WriteLine($"[Error] File not found at \"{yamlFilePath}\"");
            return;
        }

        string yaml = File.ReadAllText(yamlFilePath);

        try
        {
            PrototypeParser.Load(yaml);
            Console.WriteLine("[Success] YAML loaded and parsed");

            var allPrototypes = PrototypeRegistry.GetAll();
            Console.WriteLine($"\t[Data] YAML Parsed into {allPrototypes.Count} prototype(s)");

            foreach (var proto in allPrototypes)
            {
                Console.WriteLine($"- ID: {proto.ID}, Name: {proto.Name}");

                var components = proto.GetAllComponents();
                foreach (var comp in components)
                {
                    Console.WriteLine($"\t- Component: {comp.GetType()}");

                    foreach (var prop in comp.GetType().GetProperties())
                    {
                        var val = prop.GetValue(comp);
                        Console.WriteLine($"\t\t- {prop.Name}: ({val.GetType()}) {val}");
                    }
                }
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] Exception thrown during parsing: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}