using UnityEngine;

public class RecipeButtonInteraction : MonoBehaviour, ILeftClickable, IHoverable
{
    public void OnLeftClick()
    {
        Debug.Log("67 Mangos");
    }

    public void OnHoverEnter()
    {

    }

    public void OnHoverExit()
    {

    }
    public string HoverMessage => "Click for Discovered Recipes.";


}
