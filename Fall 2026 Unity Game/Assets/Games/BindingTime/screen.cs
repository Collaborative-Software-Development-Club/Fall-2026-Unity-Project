using UnityEngine;

public class Screen : MonoBehaviour
{
    [SerializeField] private int widthTiles = 51; // 3*16 + 3
    [SerializeField] private int heightTiles = 30; // 3*9 + 3
    [SerializeField] private GameObject childPrefab;

    void Start()
    {
        for (int i = 0; i < widthTiles; i++) {
            for (int j = 0; j < heightTiles; j++) {
                GameObject newChild = Instantiate(childPrefab, transform);
                newChild.transform.localPosition = new(i, j, 0f);
            }
        }
    }
}