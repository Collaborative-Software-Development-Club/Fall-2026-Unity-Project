using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public Item Item;

    public static GameObject CreateObj(Item item)
    {
        GameObject obj = new GameObject();
        obj.name = item.GetName() + " Holder";
        
        ItemHolder itemHolder = obj.AddComponent<ItemHolder>();
        itemHolder.Item = item;
        
        GameObject spriteObj = new GameObject();
        SpriteRenderer spriteRenderer = spriteObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.GetIcon();
        spriteRenderer.sortingOrder = 5;
        
        spriteObj.transform.SetParent(obj.transform);

        return obj;
    }
}
