using System;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    private class RecipeMachine {
        public Recipe recipe;
        public MachineData machineData;

        public override string ToString()
        {
            string output = "";
            output += recipe.ToStringInputs();
            output += " -> ";
            output += machineData.text;
            output += " -> ";
            output += recipe.ToStringOutputs();
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is RecipeMachine other)
            {
                output = recipe == other.recipe && machineData == other.machineData;
            }
            return output;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(recipe, machineData);
        }
    }
    private HashSet<RecipeMachine> foundRecipes = new HashSet<RecipeMachine>();

    private List<RecipeMachine> foundRecipesOrder = new List<RecipeMachine>();

    private static RecipeManager _instance;

    private int pageNum = 0;

    public static RecipeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogWarning("RecipeManager instance is null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        GameManager.Instance.conveyorManager.MachineAdd += (Machine machine) =>
        {
            machine.OnCraft += (Recipe recipe, MachineData machine) =>
            {
                AddToFoundRecipes(recipe, machine);
            };
        };
    }
    public bool AddToFoundRecipes(Recipe entry, MachineData source) {
        RecipeMachine current = new RecipeMachine();
        current.recipe = entry;
        current.machineData = source;
        bool output = foundRecipes.Add(current);
        
        if (output)
        {
            foundRecipesOrder.Add(current);
        }

        return output;
    }

    public string GetFoundRecipesString()
    {
        string output = "";
        foreach (RecipeMachine recipeMachine in foundRecipes)
        {
            output += recipeMachine.ToString() + "\n";
        }
        return output;
    }

    public string NextPageFoundRecipes()
    {
        string output = "";

        output += foundRecipesOrder[pageNum].ToString();

        pageNum++;

        if (pageNum >= foundRecipes.Count)
        {
            ResetPage();
        }

        return output;
    }

    public void ResetPage()
    {
        pageNum = 0;
    }
}
