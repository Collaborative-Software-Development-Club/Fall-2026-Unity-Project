using UnityEngine;

public class InteractionTest : MonoBehaviour, ILeftClickable, IRightClickable, IHoverable
{
    public void OnLeftClick()
    {
        Debug.Log("67 Mangos");
    }

    public void OnRightClick()
    {
        Debug.Log("GG Freaken Easy");
    }

    public void OnHoverEnter()
    {
        Debug.Log("Godly Tung");
    }

    public void OnHoverExit()
    {
        Debug.Log("Heavly Tung");
    }

    public string HoverMessage => "TRIPLE T SAHOUR";
}
