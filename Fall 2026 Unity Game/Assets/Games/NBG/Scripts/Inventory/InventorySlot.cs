[System.Serializable]
public class InventorySlot
{ // Carson was here
    private ItemQuantity _itemQuantity; 

    public InventorySlot(Item item, int quantity)
    {
        _itemQuantity = new ItemQuantity
        {
            Item = item,
            Quantity = quantity
        };
    }

    public InventorySlot() {
        _itemQuantity = new ItemQuantity
        {
            Item = null,
            Quantity = 0
        };
    }

    /// <summary>
    /// Resets data on Item Quantity to null for item and 0 for quantity
    /// </summary>
    public void Reset()
    {
        SetItem(null);
        SetQuantity(0);
    }

    public void SetItem(Item item)
    {
        _itemQuantity.Item = item;
    }

    public Item GetItem()
    {
        return  _itemQuantity.Item;
    }
    
    public bool AddQuantity(int amount)
    {
        if (amount <= 0)
            return false;
        
        SetQuantity(GetQuantity() + amount);
        return true;
    }

    public bool RemoveQuantity(int amount)
    {
        if (amount <= 0)
            return false;
        
        SetQuantity(GetQuantity() - amount);
        return true;
    }

    public void SetQuantity(int amount)
    {
        _itemQuantity.Quantity = amount;
    }

    public int GetQuantity()
    {
        return _itemQuantity.Quantity;
    }

    public ItemType? Type() {
        if (_itemQuantity.Item is null) return null;
        return GetItem().GetItemType();
    }

    public bool Add(InventorySlot invSlot) {
        return Add(invSlot.GetItem(), invSlot.GetQuantity());
    }

    public bool Add(Item item, int quantity) {
        if (GetQuantity() == quantity) {
            AddQuantity(quantity);
            return true;
        }
        if (GetQuantity() == 0 || item is null) {
            SetItem(item);
            AddQuantity(quantity);
            return true;
        }
        return false;
    }

    public bool IsType(ItemType? type) {
        if (type is null && GetItem() is null) return true;
        if (type !is null && GetItem() !is null && type == GetItem().GetItemType()) return true;
        return false;
    }

    public bool IsTypeAs(Item item) {
        if (item is null) return IsType(null);
        if (!item.HasData()) return IsType(null);
        // Debug.Log(item.HasData());
        // Debug.Log(item is null);
        return IsType(item.GetItemType());
    }
}