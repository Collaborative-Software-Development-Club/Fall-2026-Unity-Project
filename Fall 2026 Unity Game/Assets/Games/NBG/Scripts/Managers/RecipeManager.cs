using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    private HashSet<Recipe> foundRecipes = new HashSet<Recipe>();

    private void Start()
    {
        GameManager.Instance.conveyorManager.MachineAdd += (Machine machine) =>
        {
            machine.OnCraft += (Recipe recipe) =>
            {
                AddToFoundRecipes(recipe);
            };
        };
    }
    public bool AddToFoundRecipes(Recipe entry) {
        bool output = false;
        if (!foundRecipes.Contains(entry))
        {
            foundRecipes.Add(entry);
            output = true;
        }
        return output;
    }

    public string GetFoundRecipesString()
    {
        string output = "";
        foreach (Recipe recipe in foundRecipes)
        {
            output += recipe.ToString() + "\n";
        }
        return output;
    }
}
