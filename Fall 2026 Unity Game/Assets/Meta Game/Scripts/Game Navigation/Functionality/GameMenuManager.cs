using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] private GameDatabase database;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private float radius = 1f;
    [SerializeField] private float spawnDelay = 1f;
    [SerializeField][Tooltip("Time it takes for node to expand to full size")] private float growthTime = 1f;

    const float FULLROTATION = 2 * Mathf.PI;

    private void Start()
    {
        var gameNodes = CreateNodes();
        StartCoroutine(DisplayNodes(gameNodes));
    }

    private List<GameObject> CreateNodes()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject); // clear existing
        }

        int gamesMade = 0;
        float radOffset = FULLROTATION / database.games.Length;
        List<GameObject> gameNodes = new();

        foreach (var game in database.games)
        {
            Vector3 startingPosition = CalculatePosition(gamesMade, radOffset);
            var gameNode = Instantiate(buttonPrefab, gameObject.transform.localPosition + startingPosition, Quaternion.identity, gameObject.transform);
            gameNode.GetComponent<GameNode>().Setup(game);
            gameNode.GetComponent<SpriteRenderer>().color = game.color;
            gamesMade++;
            gameNodes.Add(gameNode);
        }

        return gameNodes;
    }

    IEnumerator DisplayNodes(List<GameObject> gameNodes)
    {
        foreach (var gameNode in gameNodes)
        {
            gameNode.GetComponent<SmoothScale>().ChangeScale(Vector3.one, growthTime);
            yield return new WaitForSeconds(spawnDelay);
        }
        
    }

    private Vector3 CalculatePosition(int gamesMade, float radOffset)
    {
        float radian = gamesMade * radOffset;
        return new Vector3(Mathf.Cos(radian) * radius, Mathf.Sin(radian) * radius, 0f);
    }
}
