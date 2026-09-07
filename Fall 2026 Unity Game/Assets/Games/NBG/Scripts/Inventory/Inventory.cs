using System.Collections.Generic;
using UnityEngine;

public struct InventoryChange
{
    public int Index;        
    public int NewQuantity;  
}
public class Inventory
{
    public InventorySlot[] slots = new InventorySlot[0];
    public Inventory(int size) {
        slots = new InventorySlot[size];
        for (int i = 0; i < size; i++) {
            slots[i] = new InventorySlot();
        }
    }

    public int AddItemToInventory(Item item, int quantity = 1)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null || !slots[i].item.Equals(item)) continue;
            
            slots[i].quantity += quantity;
            return i;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null) continue;
            
            slots[i].item = item; 
            slots[i].quantity = quantity;
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
            slots[slot].quantity += quantity;
        }

        slots[slot].item = item;
        slots[slot].quantity = quantity;
        return replace;
    }
    
    public InventorySlot RemoveFromSlot(int slot, int quantity = 1) {
        InventorySlot returning = new InventorySlot();

        if (quantity <= 0) {
            Debug.Log("Invalid quantity requested at slot " + slot);
            return returning;
        }

        if (slots[slot].quantity > quantity) {
            slots[slot].quantity -= quantity;
            returning.Add(slots[slot].item, quantity);
        } 
        else if (slots[slot].quantity > 0) {
            returning.Add(slots[slot].item, slots[slot].quantity);
            
            slots[slot].item = null;
            slots[slot].quantity = 0;
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
        change.NewQuantity = GetItemAt(itemIndex).quantity;
        
        return change;
    }

    // Return the total number of items within the inventory.
    public int GetTotalItemCount() {
        int count = 0;
        for (int i = 0; i < slots.Length; i++) {
            count += slots[i].quantity;
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
            if (slots[i].item.Equals(item)) return i;
        }
        
        return -1;
    }
}