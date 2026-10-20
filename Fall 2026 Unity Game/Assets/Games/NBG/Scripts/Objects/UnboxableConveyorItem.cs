using UnityEngine;

public class UnboxableConveyorItem : ConveyorItemRenderer, ILeftClickable
{
    [SerializeField] private ItemData packagedItem;

    public void OnLeftClick()
    {
        
    }

}
