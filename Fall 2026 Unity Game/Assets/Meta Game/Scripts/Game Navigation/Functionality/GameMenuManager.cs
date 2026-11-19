using System.Collections;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] private GameDatabase database;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private float radius = 1f;
    [SerializeField] private float spawnDelay = 1f;

    const float FULLROTATION = 2 * Mathf.PI;

    private void Start()
    {
        StartCoroutine(PopulateMenu());
    }

    IEnumerator PopulateMenu()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject); // clear existing
        }

        int gamesMade = 0;
        float radOffset = FULLROTATION / database.games.Length;

        foreach (var game in database.games)
        {
            Vector3 startingPosition = CalculatePosition(gamesMade, radOffset);
            var gameNode = Instantiate(buttonPrefab, gameObject.transform.position + startingPosition, Quaternion.identity, gameObject.transform);
            gameNode.GetComponent<GameNode>().Setup(game);
            gameNode.GetComponent<SpriteRenderer>().color = game.color;
            gamesMade++;
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private Vector3 CalculatePosition(int gamesMade, float radOffset)
    {
        float radian = gamesMade * radOffset;
        return new Vector3(Mathf.Cos(radian) * radius, Mathf.Sin(radian) * radius, gameObject.transform.position.z) ;
    }
}
