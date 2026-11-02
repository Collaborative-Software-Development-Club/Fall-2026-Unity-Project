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
        TextController.Instance.SetText(HoverMessage);

    }

    public void OnHoverExit()
    {
        Debug.Log("Heavly Tung");
        TextController.Instance.ClearText();
    }

    public string HoverMessage => "TRIPLE T SAHOUR";
}
