using UnityEngine;

public class RecipeBookTest : MonoBehaviour
{
    [SerializeField] private Recipe recipe;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print(recipe.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
