using System;
using UnityEngine;

[Serializable]
public class ItemDataQuantity
{
    [SerializeReference] public ItemData ItemData;
    public int Quantity;

    public ItemDataQuantity(ItemData itemData, int quantity)
    {
        ItemData = itemData;
        Quantity = quantity;
    }
}
