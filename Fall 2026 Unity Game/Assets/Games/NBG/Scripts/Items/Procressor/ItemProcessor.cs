using System.Collections.Generic;
using UnityEngine;

public abstract class ItemProcessor
{
    public abstract bool InsertInput(ItemData itemData);
    public abstract bool ProcessInput(Inventory input, Inventory output);
    public abstract bool CanAddItem(ItemData itemData);
    public abstract bool CanAddItem();
}
