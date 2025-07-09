using Zeta.ECS;

public class RecipeBookComponent : Component
{
    public List<RecipeLink> RecipesByInput { get; set; } = new();
    public List<RecipeLink> RecipesByOutput { get; set; } = new();
}

public struct RecipeLink {
    private HashSet<string> _keys { get; set; }
    private string _recipeId { get; set; }

    public RecipeLink(string recipeId, params string[] keys)
    {
        _keys = new HashSet<string>(keys);
        _recipeId = recipeId;
    }

    public bool Contains(string key) => _keys.Contains(key);
}