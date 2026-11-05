using System.Collections.Generic;
using UnityEngine;

public abstract class ItemProcessor 
{
    public abstract void InsertInput(ItemData itemData);
    public abstract void RemoveFromInput(ItemData itemData);
    public abstract List<ItemData> GetInput();
    public abstract List<ItemData> GetOutput();
    public abstract bool Contains(ItemData itemData);
}
