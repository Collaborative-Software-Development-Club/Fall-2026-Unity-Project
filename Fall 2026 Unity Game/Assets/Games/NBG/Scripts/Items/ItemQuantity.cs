using System;
using UnityEngine;

[Serializable]
public class ItemQuantity
{
    public Item Item;
    public int Quantity;

    public bool IsEmpty() => Item is null || Quantity <= 0;
}
