using System;
using UnityEngine;

[Serializable]
public class ItemQuantity
{
    [SerializeReference] public Item Item;
    public int Quantity;

    public bool IsEmpty() => Item is null || Quantity <= 0;

    public override string ToString()
    {
        return Item + ": " + Quantity;
    }
    
    /// <summary>
    /// Creates a item quantity with the given item and quantity
    /// </summary>
    /// <param name="item">The given item</param>
    /// <param name="quantity">The given quantity</param>
    public ItemQuantity(Item item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }

    /// <summary>
    /// Creates a item with the quantity of 1
    /// </summary>
    /// <param name="item">The given quantity</param>
    public ItemQuantity(Item item)
    {
        Item = item;
        Quantity = 1;
    }

    /// <summary>
    /// Creates a ItemQuantity with no item and a quantity of 0
    /// </summary>
    public ItemQuantity()
    {
        Item = null;
        Quantity = 0;
    }
}
