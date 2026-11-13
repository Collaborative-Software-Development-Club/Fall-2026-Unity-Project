using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class Machine : Item, IConveyorable 
{
    private const int ProcessTicks = 2;

    private MachineData machineData => data as MachineData;

    protected Inventory Input;
    protected Inventory Output;

    private List<ItemQuantity> _pendingOutputs;
    private int _ticksRemaining;
    public Action<Recipe, MachineData> OnCraft;

    private Dictionary<ItemData, List<Recipe>> _currentAvailableRecipes;
    private Recipe _currentCraftableRecipe;
    
    private ItemProcessor _processor;
    
    /// <summary>
    /// Function for retrieving the machine's data
    /// </summary>
    /// <returns></returns>
    public override ItemData GetData()
    {
        return machineData;
    }

    // Function for retrieving the type this machine is.
    public machineType GetMachineType() 
    {
        return machineData.processType;
    }

    // Function for retrieving the input inventory of this machine.
    public Inventory GetInputInventory()
    {
        return Input;
    }
    // Function for retrieving the output inventory of this machine.
    public Inventory GetOutputInventory()
    {
        return Output;
    }
    // Function for retrieving items found within Input.
    public Item GetInputFromSlot(int slot)
    { 
        return Input.GetItemAt(slot).GetItem();
    }

    // Function for retrieving items found within Output.
    public Item GetOutputFromSlot(int slot)
    {
        return Output.GetItemAt(slot).GetItem();
    }

    /// <summary>
    /// Function for adding an item to the input inventory of this machine.
    /// Returns true if successful, false if the input inventory is full.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="quantity">The quantity of the item to add. Defaults to 1.</param>
    /// <returns>
    /// True if successful; otherwise false.
    /// </returns>
    public bool AddItemToInput(Item item, int quantity = 1)
    {
        if (item == null) return false;

        // Items that cannot be processed are sent straight through.
        if (item is not Processable) 
            return AddItemToOutput(item, quantity);
        
        int newIndex = -1;
        
        if(_processor.CanAddItem(item.GetData())) 
            newIndex = Input.AddItemToInventory(item, quantity);
        
        Debug.Log("what the frick");
        
        return newIndex > -1;
    }

    public bool AddItemToInput(ItemQuantity itemQuantity)
    {
        return AddItemToInput(itemQuantity.Item, itemQuantity.Quantity);
    }

    /// <summary>
    /// Function for adding an item to the output inventory of this machine.
    /// Returns true if successful, false if the output inventory is full.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="quantity">The quantity of the item to add. Defaults to 1.</param>
    /// <returns>
    /// True if successful; otherwise false.
    /// </returns>
    public bool AddItemToOutput(Item item, int quantity = 1)
    {
        int newIndex = Output.AddItemToInventory(item, quantity);

        return newIndex > -1;
    }

    /// <summary>
    /// Function for removing an item from the input inventory of this machine.
    /// Returns true if successful, false if the input inventory is empty or does not contain the item.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <param name="quantity">The quantity of the item to remove. Defaults to 1.</param>
    /// <returns>
    /// True if successful; otherwise false.
    /// </returns>
    public bool RemoveItemFromInput(Item item, int quantity = 1)
    {
        int index = -1;
        for (int i = 0; i < Input.Length && index == -1; i++)
        {
            var slotItem = Input.GetItemAt(i);
            if (slotItem != null && slotItem.GetItem() == item) 
                index = i;
        }

        if (index == -1)
        {
            Debug.LogWarning($"Item {item.GetName()} not found in input inventory!");
            return false;
        }

        InventorySlot removed = Input.RemoveFromSlot(index, quantity);

        return removed != null && removed.GetQuantity() > 0;
    }

    /// <summary>
    /// Function for removing an item from the output inventory of this machine.
    /// Returns true if successful, false if the output inventory is empty or does not contain the item.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <param name="quantity">The quantity of the item to remove. Defaults to 1.</param>
    /// <returns>
    /// True if successful; otherwise false.
    /// </returns>

    // TODO: Need to change back to RemoveItemFromInventory, RemoveFromSlot is a very temp fix
    public InventorySlot RemoveItemFromOutput(int index, int quantity = 1)
    {
        var removed = Output.RemoveFromSlot(index, quantity);

        return removed;
    }
    public Machine(MachineData itemData)
    {
        data = itemData;
        
        int inputSize = Mathf.Max(0, machineData.inputCount);
        Input = new Inventory(inputSize);

        int outputSize = Mathf.Max(0, machineData.outputCount);
        Output = new Inventory(outputSize);

        if (machineData.processType == machineType.None)
            Debug.LogWarning($"{machineData.name} has no machine type.");

        bool areRecipesOneToOne = machineData.recipes.All(recipe => recipe.GetInputs.Count <= 1 && recipe.GetOutputs.Count <= 1);

        if (areRecipesOneToOne)
            _processor = new SingleItemProcessor(machineData.recipes);
        else
            _processor = new MultiItemProcessor();
    }
    public Machine(MachineData itemData, string itemName) : this(itemData)
    {
        Name = itemName;
    }

    public bool PerformOperation()
    {
        // TODO: Add a timer / operation time
        return _processor.ProcessInput(Input, Output);
    }

    // TODO: Abstract out in the future to have a in-between to the conveyor ( THIS IS GOOD ENOUGH FOR THE PRESENTATION )

    #region ConveyorInteraction 
    public bool AcceptsConveyorItems { get; set; } = true;

    public int GetSize() => Input.Length;
    public bool AddItemQuantityToPart(ItemQuantity itemQuantity)
    {
        if (!AcceptsConveyorItems) return false;
        return AddItemToInput(itemQuantity);
    }

    public ItemQuantity RemoveLastItemFromPart()
    {
        for (int i = 0; i < Output.Length; i++)
        {
            InventorySlot slot = Output.GetItemAt(i);
            if (slot?.GetItem() == null || slot.GetQuantity() <= 0) continue;

            Item item = slot.GetItem();
            int quantity = slot.GetQuantity();
            Output.RemoveFromSlot(i, quantity);
            item?.AddHeat(machineData.heatPerPass);
            return new ItemQuantity(item, quantity);
        }

        return null;
    }

    public bool DoAction(IConveyorable nextConveyorable)
    {
        if (!AcceptsConveyorItems) return false;

        bool crafted = PerformOperation();

        if (nextConveyorable == null || !nextConveyorable.HasSpace())
            return crafted;

        ItemQuantity outputItem = RemoveLastItemFromPart();
        if (outputItem == null || outputItem.IsEmpty())
            return crafted;

        if (nextConveyorable.AddItemQuantityToPart(outputItem))
            return true;

        AddItemToOutput(outputItem.Item, outputItem.Quantity);
        return crafted;
    }

    public bool HasSpace()
    {
        if (!_processor.CanAddItem()) return false; 
        
        for (int i = 0; i < Input.Length; i++)
        {
            InventorySlot slot = Input.GetItemAt(i);
            if (slot == null || slot.GetItem() == null || slot.GetQuantity() <= 0)
                return true;
        }

        return false;
    }

    #endregion
}
