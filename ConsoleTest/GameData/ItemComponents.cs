using Zeta.ECS;

public class ItemComponent : Component
{
    public string Description { get; set; } = "Not important enough to get a description I guess...";
    public int Tier { get; set; } = 0;
    public ItemRarity Rarity { get; set; } = ItemRarity.Basic;
}

public enum ItemRarity
{
    Basic,
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Mythical
}