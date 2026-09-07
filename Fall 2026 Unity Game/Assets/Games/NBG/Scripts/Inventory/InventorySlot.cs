[System.Serializable]
public class InventorySlot
{ // Carson was here
    public Item item;
    public int quantity;

    public InventorySlot(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public InventorySlot() {
        item = null;
        quantity = 0;
    }

    public ItemType? Type() {
        if (item is null) return null;
        return item.GetItemType();
    }

    public bool Add(InventorySlot invSlot) {
        return Add(invSlot.item, invSlot.quantity);
    }

    public bool Add(Item item, int quantity) {
        if (this.quantity == quantity) {
            this.quantity += quantity;
            return true;
        }
        if (this.quantity == 0 || item is null) {
            this.item = item;
            this.quantity += quantity;
            return true;
        }
        return false;
    }

    public bool IsType(ItemType? type) {
        if (type is null && item is null) return true;
        if (type !is null && item !is null && type == item.GetItemType()) return true;
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