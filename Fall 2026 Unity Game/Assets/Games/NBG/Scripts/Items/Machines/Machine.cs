using System;
using System.Collections.Generic;
using UnityEngine;

public class Machine : Item, IConveyorable 
{
    private const int ProcessTicks = 2;

    private MachineData machineData => data as MachineData;

    protected Inventory Input;
    protected Inventory Output;

    private List<ItemQuantity> _pendingOutputs;
    private int _ticksRemaining;
    
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
        
        int newIndex = Input.AddItemToInventory(item, quantity);
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
    }
    public Machine(MachineData itemData, string itemName) : this(itemData)
    {
        Name = itemName;
    }

    public bool PerformOperation()
    {
        if (_pendingOutputs != null)
        {
            _ticksRemaining--;
            if (_ticksRemaining > 0) return false;

            foreach (ItemQuantity pending in _pendingOutputs)
                AddItemToOutput(pending.Item, pending.Quantity);

            _pendingOutputs = null;
            return true;
        }

        List<ItemDataQuantity> inputs = Input.GetInventoryAsItemDataQuantityList();

        if (machineData.recipes != null)
        {
            foreach (Recipe recipe in machineData.recipes)
            {
                if (recipe == null || recipe.GetInputs == null || recipe.GetInputs.Count == 0) continue;

                List<ItemDataQuantity> output = recipe.Process(inputs);
                if (output == null) continue;
                if (!CanFitOutputs(output)) return false;

                TryStartCraft(recipe);
                return false;
            }
        }

        PassThroughIfUnusable(inputs);
        return false;
    }

    private bool TryStartCraft(Recipe recipe)
    {
        List<ItemQuantity> created = new List<ItemQuantity>();
        if (recipe.GetOutputs == null) return false;

        foreach (ItemDataQuantity itemDataQuantity in recipe.GetOutputs)
        {
            if (itemDataQuantity == null || itemDataQuantity.ItemData == null || itemDataQuantity.Quantity <= 0)
                continue;

            Item createdItem = ItemFactory.CreateItemFromSO(itemDataQuantity.ItemData);
            if (createdItem == null) return false;

            created.Add(new ItemQuantity(createdItem, itemDataQuantity.Quantity));
        }

        if (!ConsumeInputs(recipe.GetInputs)) return false;

        _pendingOutputs = created;
        _ticksRemaining = ProcessTicks;
        return true;
    }

    private void PassThroughIfUnusable(List<ItemDataQuantity> inputs)
    {
        if (inputs == null || inputs.Count == 0 || CouldMatchAnyRecipe(inputs)) return;

        for (int i = 0; i < Input.Length; i++)
        {
            InventorySlot slot = Input.GetItemAt(i);
            if (slot?.GetItem() == null || slot.GetQuantity() <= 0) continue;
            if (!CanFit(slot.GetItem(), slot.GetQuantity())) return;

            Item item = slot.GetItem();
            int quantity = slot.GetQuantity();
            Input.RemoveFromSlot(i, quantity);
            AddItemToOutput(item, quantity);
            return;
        }
    }

    private bool CouldMatchAnyRecipe(List<ItemDataQuantity> inputs)
    {
        if (machineData.recipes == null) return false;

        foreach (Recipe recipe in machineData.recipes)
        {
            if (recipe?.GetInputs == null || recipe.GetInputs.Count == 0) continue;
            if (CouldMatch(recipe, inputs)) return true;
        }

        return false;
    }

    private static bool CouldMatch(Recipe recipe, List<ItemDataQuantity> inputs)
    {
        if (inputs.Count > recipe.GetInputs.Count) return false;

        bool[] used = new bool[recipe.GetInputs.Count];

        foreach (ItemDataQuantity held in inputs)
        {
            bool found = false;

            for (int i = 0; i < recipe.GetInputs.Count; i++)
            {
                if (used[i]) continue;

                ItemDataQuantity required = recipe.GetInputs[i];
                if (required == null || held.ItemData != required.ItemData || held.Quantity > required.Quantity)
                    continue;

                used[i] = true;
                found = true;
                break;
            }

            if (!found) return false;
        }

        return true;
    }

    private bool ConsumeInputs(List<ItemDataQuantity> required)
    {
        if (required == null || !HasInputs(required)) return false;

        foreach (ItemDataQuantity requirement in required)
        {
            int remaining = requirement.Quantity;

            for (int i = 0; i < Input.Length && remaining > 0; i++)
            {
                InventorySlot slot = Input.GetItemAt(i);
                if (slot?.GetItem() == null || slot.GetItem().GetData() != requirement.ItemData) continue;

                int take = Mathf.Min(remaining, slot.GetQuantity());
                Input.RemoveFromSlot(i, take);
                remaining -= take;
            }
        }

        return true;
    }

    private bool HasInputs(List<ItemDataQuantity> required)
    {
        foreach (ItemDataQuantity requirement in required)
        {
            if (requirement == null || requirement.ItemData == null) return false;

            int found = 0;
            for (int i = 0; i < Input.Length; i++)
            {
                InventorySlot slot = Input.GetItemAt(i);
                if (slot?.GetItem() == null || slot.GetItem().GetData() != requirement.ItemData) continue;
                found += slot.GetQuantity();
            }

            if (found < requirement.Quantity) return false;
        }

        return true;
    }

    private bool CanFitOutputs(List<ItemDataQuantity> outputs)
    {
        if (outputs == null) return true;

        int empty = CountEmpty(Output);
        int needed = 0;

        foreach (ItemDataQuantity output in outputs)
        {
            if (output == null || output.ItemData == null || output.Quantity <= 0) continue;
            if (OutputHasStack(output.ItemData)) continue;
            needed++;
        }

        return needed <= empty;
    }

    private bool CanFit(Item item, int quantity)
    {
        if (item == null || quantity <= 0) return false;

        int empty = 0;
        for (int i = 0; i < Output.Length; i++)
        {
            InventorySlot slot = Output.GetItemAt(i);
            if (slot == null || slot.GetItem() == null || slot.GetQuantity() <= 0)
            {
                empty++;
                continue;
            }

            if (slot.GetItem().Equals(item) && slot.GetItem().GetStackable())
                return true;
        }

        return empty > 0;
    }

    private bool OutputHasStack(ItemData itemData)
    {
        for (int i = 0; i < Output.Length; i++)
        {
            InventorySlot slot = Output.GetItemAt(i);
            if (slot?.GetItem() == null || slot.GetQuantity() <= 0) continue;
            if (slot.GetItem().GetData() == itemData && slot.GetItem().GetStackable())
                return true;
        }

        return false;
    }

    private static int CountEmpty(Inventory inventory)
    {
        int empty = 0;
        for (int i = 0; i < inventory.Length; i++)
        {
            InventorySlot slot = inventory.GetItemAt(i);
            if (slot == null || slot.GetItem() == null || slot.GetQuantity() <= 0)
                empty++;
        }

        return empty;
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
