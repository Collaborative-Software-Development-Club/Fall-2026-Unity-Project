using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Game/NBG/Recipe/Base Recipe")]

public class Recipe : ScriptableObject
{
    [SerializeField] private ItemData testItem;
    [SerializeField] private List<ItemDataQuantity> inputs;
    [SerializeField] private List<ItemDataQuantity> outputs;
    
    public List<ItemDataQuantity> GetInputs => inputs;
    public List<ItemDataQuantity> GetOutputs => outputs;

    /// <summary>
    /// Checks to see if the items given are the same as the inputs and gives the outputs if they are and null if they aren't
    /// </summary>
    /// <param name="items">The items to check to see if they are inputs</param>
    /// <returns>Outputs if match or Null if not match</returns>
    public List<ItemDataQuantity> Process(List<ItemDataQuantity> items)
    {
        if (items.Count != inputs.Count) return null;

        var isMatch = inputs.All(input => 
            items.Any(item => Equals(item.ItemData, input.ItemData) && item.Quantity == input.Quantity)
        );

        return isMatch ? outputs : null;
    }
}

