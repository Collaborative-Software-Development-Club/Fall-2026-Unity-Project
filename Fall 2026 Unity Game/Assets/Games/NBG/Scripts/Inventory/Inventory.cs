using System.Collections.Generic;
using UnityEngine;

public struct InventoryChange
{
    public int Index;        
    public int NewQuantity;  
}
public class Inventory
{
    private int _emptySlots = 0;
    
    public InventorySlot[] slots = new InventorySlot[0];
    public Inventory(int size) {
        slots = new InventorySlot[size];
        for (int i = 0; i < size; i++) {
            slots[i] = new InventorySlot();
        }

        _emptySlots = size;
    }

    /// <summary>
    /// Adds the item to the inventory with the given quantity
    /// <returns> the new index </returns>
    /// </summary>
    public int AddItemToInventory(Item item, int quantity = 1)
    {
        // Could be improved by tracking open slots or turning inventory into dictionary, shouldn't loop 2x
        // Or could even track a open slot during the 1st loop and use it if no item is found
        for (int i = 0; i < slots.Length; i++)
        {
            
            if (slots[i].GetItem() == null || !slots[i].GetItem().Equals(item) || (!slots[i].GetItem().GetStackable() && slots[i].GetItem().Equals(item))) continue;
            
            slots[i].AddQuantity(quantity);
            return i;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetItem() != null) continue;
            
            slots[i].SetItem(item);
            slots[i].SetQuantity(quantity);
            return i;
        }

        return -1;
    }

    // This doesn't make since the slot should never be null
    public InventorySlot AddToSlot(int slot, Item item, int quantity=1) {
        InventorySlot replace = slots[slot];
        if (slots[slot] == null) {
            Debug.LogWarning($"Slot {slot} is null!" );
            return replace;
        }
        if (slots[slot].IsTypeAs(item)) {
            slots[slot].AddQuantity(quantity);
        }

        slots[slot].SetItem(item);
        slots[slot].SetQuantity(quantity);
        _emptySlots -= 1;
        return replace;
    }
    
    public InventorySlot RemoveFromSlot(int slot, int quantity = 1) {
        InventorySlot returning = new InventorySlot();

        if (quantity <= 0) {
            return returning;
        }

        if (slots[slot].GetQuantity() > quantity) {
            slots[slot].RemoveQuantity(quantity);
            returning.Add(slots[slot].GetItem(), quantity);
        } 
        else if (slots[slot].GetQuantity() > 0) {
            returning.Add(slots[slot].GetItem(), slots[slot].GetQuantity());
            
            slots[slot].Reset();
            _emptySlots += 1;
        }
        
        return returning;
    }

    /// <summary>
    /// Removes up to <paramref name="quantity"/> of a specific <paramref name="item"/> from the inventory.
    /// Returns a list of changes, each with the slot index and the new quantity after removal.
    ///
    /// DOESN'T WORK
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <param name="quantity">The maximum quantity to remove. Defaults to 1.</param>
    /// <returns>
    /// A list of InventoryChange structs containing the slot index and the updated quantity.
    /// </returns>
    public InventoryChange RemoveItemFromInventory(Item item, int quantity = 1)
    {
        InventoryChange change = new InventoryChange();
        change.Index = -1;
        
        int itemIndex = GetItemAtFirstFoundIndex(item);

        if (itemIndex < 0) return change;

        change.Index = itemIndex;
        RemoveFromSlot(itemIndex, quantity);
        change.NewQuantity = GetItemAt(itemIndex).GetQuantity();
        
        return change;
    }

    // Return the total number of items within the inventory.
    public int GetTotalItemCount() {
        int count = 0;
        for (int i = 0; i < slots.Length; i++) {
            count += slots[i].GetQuantity();
        }
        return count;
    }

    public int Length {
        get {
            return slots.Length;
        }
    }

    public InventorySlot GetItemAt(int slot) {
        if (slot > slots.Length) return null;
        return slots[slot];
    }

    /// <summary>
    /// Gets the first instance of the passed in item.
    /// </summary>
    /// <param name="item">The item to look for.</param>
    /// <returns>Returns the index or -1 if not found.</returns>
    public int GetItemAtFirstFoundIndex(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetItem().Equals(item)) return i;
        }
        
        return -1;
    }

    public bool HasEmptySlots()
    {
        return _emptySlots > 0;
    }

    public void PrintInventory()
    {
        string printMessage = "";
        int totalItems = Length;
        
        foreach (InventorySlot slot in slots)
        {
            printMessage += slot.GetItem() != null ? totalItems-- : slot.GetItem() + " | " + slot.GetQuantity() + "\n";
        }
        
        Debug.Log("Inventory:\n" + printMessage + "\nTotal Items: "  + totalItems);
    }

    public InventorySlot[] GetInventorySlots()
    {
        return slots;
    }

    /// <summary>
    /// Converts the current inventory into a Item Quantity list to be used in other systems
    /// </summary>
    /// <returns>A Item Quantity List</returns>
    public List<ItemQuantity> GetInventoryAsItemQuantityList()
    {
        List<ItemQuantity> temp = new List<ItemQuantity>();

        for (int i = 0; i < Length ; i++)
            temp.Add(slots[i].GetItemQuantity());

        return temp;
    }

    /// <summary>
    /// Converts the current inventory into a Item Data Quantity list to be used in other systems
    /// </summary>
    /// <returns>A Item Quantity List</returns>
    public List<ItemDataQuantity> GetInventoryAsItemDataQuantityList()
    {
        List<ItemDataQuantity> temp = new List<ItemDataQuantity>();

        for (int i = 0; i < Length; i++)
            temp.Add(new ItemDataQuantity(slots[i].GetItemQuantity().Item.GetData(), slots[i].GetItemQuantity().Quantity));

        return temp;
    }
}