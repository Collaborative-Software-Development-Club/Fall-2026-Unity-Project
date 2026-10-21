using System;

public class UnboxableConveyorItem : ConveyorItemRenderer, IRightClickable 
{
    public Action OnRightClickEvent;

    public void OnRightClick()
    {
        OnRightClickEvent?.Invoke();
    }
}
