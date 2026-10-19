using System;

[Serializable]
public class ItemQuantity
{
    public Item Item;
    public int Quantity;

    public bool IsEmpty() => Item is null || Quantity <= 0;

    public override string ToString()
    {
        return Item + ": " + Quantity;
    }
}
