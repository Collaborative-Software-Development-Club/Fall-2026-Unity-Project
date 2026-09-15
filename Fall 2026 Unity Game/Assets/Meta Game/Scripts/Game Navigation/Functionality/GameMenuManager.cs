using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] private GameDatabase database;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject buttonPrefab;

    private void Start()
    {
        PopulateMenu();
    }

    private void PopulateMenu()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject); // clear existing
        }

        foreach (var game in database.games)
        {
            var buttonGO = Instantiate(buttonPrefab, contentParent.transform);
            var gameButton = buttonGO.GetComponent<GameButton>();
            gameButton.Setup(game);
        }
    }
}
