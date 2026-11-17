using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SingleItemProcessor : ItemProcessor 
{
    private List<Recipe> _recipes;
    private Dictionary<ItemData, ItemData> _items = new();

    private ItemData _itemToProcess = null;
    
    
    public SingleItemProcessor(List<Recipe> recipes)
    {
        _recipes = recipes;
        
        // Recipes should be assumed to have a 1 to 1 ratio due to the nature of this function
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
        Debug.Log(_itemToProcess);
        if (_itemToProcess) return false;
        Debug.Log("Please insert!");
        
        _itemToProcess =  itemData;
        return true;
    }

    public override bool ProcessInput(Inventory Input, Inventory Output)
    {
        if (!_itemToProcess) return false;

        InventorySlot slot = Input.GetItemAt(0);
        if (slot == null || slot.GetItem() == null || slot.GetQuantity() <= 0)
            return false;

        ItemData outputData = _items.GetValueOrDefault(_itemToProcess, _itemToProcess);
        Item outputItem = ItemFactory.CreateItemFromSO(outputData);
        if (outputItem == null) return false;

        // Adding to Output raises OnItemAdded, which is what shows the item leaving.
        int addedIndex = Output.AddItemToInventory(outputItem);
        if (addedIndex < 0) return false;

        Input.RemoveFromSlot(0, slot.GetQuantity());
        _itemToProcess = null;
        return true;
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
