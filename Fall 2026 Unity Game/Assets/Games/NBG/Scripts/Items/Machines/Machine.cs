using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Machine : Item
{
    private MachineData machineData => data as MachineData;

    protected Inventory Input;
    protected Inventory Output;

    private MachineFunctionality _machineFunctionality;
    
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
        int newIndex = Input.AddItemToInventory(item, quantity);

        return newIndex > -1;
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

        switch (machineData.processType)
        {
            case machineType.None:
                Debug.Log("No machine type!");
                break;
            case machineType.Furnace : break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public Machine(MachineData itemData, string itemName) : this(itemData)
    {
        Name = itemName;
    }

    public async Task<bool> PerformOperation()
    {
        bool found = false;
        
        // TODO: Make more peformant eventually
        foreach (var recipe in machineData.recipes)
        {
            var output = recipe.Process(Input.GetInventoryAsItemDataQuantityList());
            
            if (output == null) continue;

            found = true;

            await Task.Delay(1000);
            foreach (var itemDataQuantity in output)
                AddItemToOutput(ItemFactory.CreateItemFromSO(itemDataQuantity.ItemData), itemDataQuantity.Quantity);

            break;
        }

        return found;
    }
}