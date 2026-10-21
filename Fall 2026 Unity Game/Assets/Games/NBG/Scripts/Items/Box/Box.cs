using UnityEngine;


public class Box : Item 
{
    public static Item Unbox(Box box)
    {
        return ItemFactory.CreateItemFromSO(box.GetData());
    }
    
    public override ItemData GetData()
    {
        return data;
    }
}
