using UnityEngine;

public class RecipeButtonInteraction : MonoBehaviour, ILeftClickable, IHoverable
{
    public void OnLeftClick()
    {
        string recipes = GameManager.Instance.RecipeManager.NextPageFoundRecipes();
        
        if (recipes.Equals(""))
        {
            recipes = "No Discovered Recipes.";
        }
        TextController.Instance.SetText(recipes);
    }

    public void OnHoverEnter()
    {
        TextController.Instance.SetText(HoverMessage);
    }

    public void OnHoverExit()
    {
        string currentText = TextController.Instance.GetText();
        if (currentText.Equals(HoverMessage))
            TextController.Instance.ClearText();
        GameManager.Instance.RecipeManager.ResetPage();
    }
    public string HoverMessage => "Click for Discovered Recipes.";


}
