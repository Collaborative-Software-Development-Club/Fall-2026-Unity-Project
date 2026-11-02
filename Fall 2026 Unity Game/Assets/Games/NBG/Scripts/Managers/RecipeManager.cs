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
    }
    private HashSet<RecipeMachine> foundRecipes = new HashSet<RecipeMachine>();

    private static RecipeManager _instance;

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
    }
    private void Start()
    {
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
        bool output = false;
        if (!foundRecipes.Contains(current))
        {
            foundRecipes.Add(current);
            output = true;
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
}
