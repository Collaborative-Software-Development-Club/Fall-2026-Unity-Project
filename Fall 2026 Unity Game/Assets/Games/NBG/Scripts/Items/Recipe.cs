using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Recipe : ScriptableObject
{
    [SerializeField] private List<ItemQuantity> inputs;
    [SerializeField] private List<ItemQuantity> outputs;
    
    public List<ItemQuantity> GetInputs => inputs;
    public List<ItemQuantity> GetOutputs => outputs;

    /// <summary>
    /// Checks to see if the items given are the same as the inputs and gives the outputs if they are and null if they aren't
    /// </summary>
    /// <param name="items">The items to check to see if they are inputs</param>
    /// <returns>Outputs if match or Null if not match</returns>
    public List<ItemQuantity> Process(List<ItemQuantity> items)
    {
        if (items.Count != inputs.Count) return null;

        var isMatch = inputs.All(input => 
            items.Any(item => Equals(item.Item, input.Item) && item.Quantity == input.Quantity)
        );

        return isMatch ? outputs : null;
    }
}

