using Zeta.ECS;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

public class RecipeComponent : Component
{
    public List<InputData> Inputs { get; set; } = new();
    public List<OutputData> Outputs { get; set; } = new();
    
    public class InputData
    {
        public string ItemId { get; set; } = "invalid";
        public int Quanity { get; set; } = 1;
    }

    public class OutputData
    {
        public int Weight { get; set; } = 1;
        public List<OutputItem> Items { get; set; } = new();
        
        class OutputItem 
        {
            public string ItemId { get; set; } = "invalid";
            public int Quantity { get; set; } = 1;
        }
    }
}

public class PowerCraftComponent : Component
{
    public string ToolType { get; set; } = "Invalid";
    public int Power { get; set; } = 6;
}

public class SkillRequirementComponent : Component
{
    public string SkillType { get; set; } = "Invalid";

    [YamlMember(Alias = "min-level")]
    public int MinimumLevel { get; set; } = 1;
}

public class ToolRequirementComponent : Component
{
    public string ToolType { get; set; } = "Invalid";

    [YamlMember(Alias = "min-tier")]
    public int MinimumTier { get; set; } = 0;
}

public class StationRequirementComponent : Component
{
    public string StationType { get; set; } = "Invalid";

    [YamlMember(Alias = "min-tier")]
    public int MinimumTier { get; set; } = 1;
}