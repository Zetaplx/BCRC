using Zeta.ECS;

[ComponentKey("rank")]
public class RankComponent : Component
{
    public int Tier { get; set; } = 1;
    public ItemRank Rank { get; set; } = ItemRank.Common;

    public enum ItemRank
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythical
    }
}

[ComponentKey("storable")]
public class StorableComponent : Component
{
    public StorageType Size { get; set; } = StorageType.Small;

    [ComponentProperty("inv-stack")]
    public int InventoryStackSize { get; set; } = 1;

    [ComponentProperty("bank-stack")]
    public int BankStackSize { get; set; } = 1;

    public enum StorageType
    {
        Small,
        Cargo
    }
}

[ComponentKey("tool")]
public class ToolDescriptionComponent : Component
{
    public Professions Profession { get; set; }
    public int Power { get; set; } = 6;

    public enum Professions
    {
        Carpentry,
        Farming,
        Fishing,
        Foraging,
        Forestry,
        Hunting,
        Leatherworking,
        Masonry,
        Mining,
        Scholar,
        Smithing,
        Tailoring
    }
}