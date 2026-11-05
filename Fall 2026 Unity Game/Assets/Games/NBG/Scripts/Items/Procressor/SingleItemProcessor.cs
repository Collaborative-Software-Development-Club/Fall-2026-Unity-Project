using System.Collections.Generic;
using UnityEngine;

public class SingleItemProcessor : ItemProcessor 
{
    private List<Recipe> _recipes;
    private Dictionary<ItemData, ItemData> _items = new();

    private ItemData _itemToProcess = null;
    
    
    public SingleItemProcessor(List<Recipe> recipes)
    {
        _recipes = recipes;
        
        // Recipes should be assumed to have a 1 -> 1 ratio due to the nature of this function
        foreach (Recipe recipe in _recipes)
        {
            _items[recipe.GetInputs[0].ItemData] = recipe.GetOutputs[0].ItemData;
            
            if (recipe.GetInputs.Count > 1)
                Debug.LogWarning(recipe.name + " has more than 1 input in a single item processor");
            if (recipe.GetOutputs.Count > 1)
                Debug.LogWarning(recipe.name + " has more than 1 output in a single item processor");
        }
    }
    
    public override bool InsertInput(ItemData itemData)
    {
        // If it can't get the output it just puts in the input
        if (_itemToProcess) return false;
        
        _itemToProcess =  itemData;
        return true;
    }

    public override bool ProcessInput(Inventory Input, Inventory Output)
    {
        if (!_itemToProcess) return false;

        // In theory this should work since the slot should only have 1 item :pray:
        Input.RemoveFromSlot(0, Input.GetItemAt(0).GetQuantity());
        int removedIndex = Output.AddItemToInventory(ItemFactory.CreateItemFromSO(_items.GetValueOrDefault(_itemToProcess, _itemToProcess)));
        _itemToProcess = null;

        return removedIndex > 0;
    }

    public override bool CanAddItem(ItemData itemData)
    {
        return !_itemToProcess;
    }

    public override bool CanAddItem()
    {
        return !_itemToProcess;
    }
}
