using UnityEngine;


public class Box : Item 
{
    public Box(BoxData boxData)
    {
        data = boxData;
    } 
    public static Item Unbox(Box box)
    {
        return ItemFactory.CreateItemFromSO(box.GetData());
    }
    
    public override ItemData GetData()
    {
        return data;
    }
}
