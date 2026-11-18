using UnityEngine;

public class MultiItemProcessor : ItemProcessor
{
    public MultiItemProcessor()
    {
        Debug.LogWarning("Not implemented");
    }
    
    public override bool InsertInput(ItemData itemData)
    {
        Debug.LogWarning("Not implemented");
        return false;
    }

    public override bool ProcessInput(Inventory Input, Inventory Output)
    {
        Debug.LogWarning("Not implemented");
        return false;
    }

    public override bool CanAddItem(ItemData itemData)
    {
        Debug.LogWarning("Not implemented");
        return false;
    }

    public override bool CanAddItem()
    {
        Debug.LogWarning("Not implemented");
        return false;
    }
}
