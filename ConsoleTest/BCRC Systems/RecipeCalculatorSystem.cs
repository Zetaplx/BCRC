using System.Security.Cryptography.X509Certificates;
using Zeta.ECS;

public class RecipeCalculatorSystem : ProtoSystem
{
    public static RecipeTree CalculateRecipeTree(string itemId, int quanity)
    {
        if (!PrototypeRegistry.TryGetAllWithComponent<RecipeComponent>(out var recipes))
            throw new Exception("Unable to find any recipes in the prototype registry.");

        var tree = new RecipeTree();


        return tree;
    }

    public static class RecipeBook
    {
        private static List<(HashSet<string> inputs, string protoID)> recipesByInput { get; set; } = new();
        private static List<(HashSet<string> outputs, string protoID)> recipesByOutput { get; set; } = new();

        static RecipeBook()
        {
            InitializeRecipes();
        }

        private static void InitializeRecipes()
        {
            if (!PrototypeRegistry.TryGetAllWithComponent<RecipeComponent>(out var recipes))
                throw new Exception("Unable to find recipes in prototype registry");

            recipesByInput = new List<(HashSet<string>, string)>();
            recipesByOutput = new List<(HashSet<string>, string)>();

            foreach (var recipe in recipes)
            {
                if (!recipe.TryGetComponent<RecipeComponent>(out var comp))
                    throw new Exception($"Prototype {recipe.ID} incorrectly designated as recipe. Missing component.");

                var inputSet = new HashSet<string>();
                foreach (var input in comp.Inputs) inputSet.Add(input.ItemId);

                var outputSet = new HashSet<string>();
                foreach (var staticOut in comp.StaticOutputs) outputSet.Add(staticOut.ItemId);
                foreach (var rangeOut in comp.RangeOutputs) outputSet.Add(rangeOut.ItemId);
                foreach (var bucketOut in comp.BucketOutputs)
                {
                    foreach (var bucket in bucketOut.Buckets)
                    {
                        foreach (var bucketStaticOut in bucket.StaticOutputs) outputSet.Add(bucketStaticOut.ItemId);
                        foreach (var bucketRangeOut in bucket.RangeOutputs) outputSet.Add(bucketRangeOut.ItemId);
                    }
                }

                recipesByInput.Add((inputSet, recipe.ID));
            }
        }

        public static bool TryGetRecipeByInput(string input, out List<string> protoIDs)
        {
            protoIDs = new List<string>();
            foreach (var recipe in recipesByInput)
            {
                if (recipe.inputs.Contains(input))
                    protoIDs.Add(recipe.protoID);
            }

            return protoIDs.Count > 0;
        }

        public static bool TryGetRecipeByOutput(string output, out List<string> protoIDs) {
            protoIDs = new List<string>();
            foreach (var recipe in recipesByOutput)
            {
                if (recipe.outputs.Contains(output))
                    protoIDs.Add(recipe.protoID);
            }

            return protoIDs.Count > 0;
        }
    }

    public class RecipeTree
    {

    }

    public class RecipeTreeNode
    {
        
    }
}