using UnityEngine;

public class RecipeButtonInteraction : MonoBehaviour, ILeftClickable, IHoverable
{
    public void OnLeftClick()
    {
        TextController.Instance.removePriorityFromQueue(0);

        string recipes = GameManager.Instance.RecipeManager.NextPageFoundRecipes();
        
        if (recipes.Equals(""))
            recipes = "No Discovered Recipes.";
        TextController.Instance.QueueNewText(recipes, 0);
    }

    public void OnHoverEnter()
    {
        TextController.Instance.removePriorityFromQueue(0);
        TextController.Instance.QueueNewText(HoverMessage, 0);
    }

    public void OnHoverExit()
    {
        TextController.Instance.removeFromQueue(HoverMessage);
        GameManager.Instance.RecipeManager.ResetPage();
    }
    public string HoverMessage => "Click for Discovered Recipes.";


}
