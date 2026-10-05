using UnityEngine;

namespace BindingTime {
    public class TileManager : MonoBehaviour
    {
        [SerializeField] private int widthTiles = 51; // 3*16 + 3
        [SerializeField] private int heightTiles = 30; // 3*9 + 3
        [SerializeField] private GameObject childPrefab;
        private GameObject[,] tileGrid; 

        void Start() {
            //Instantiate tiles
            tileGrid = new GameObject[heightTiles, widthTiles];
            for (int j = 0; j < heightTiles; j++) {
                for (int i = 0; i < widthTiles; i++) {
                    tileGrid[j,i] = Instantiate(childPrefab, transform);
                    tileGrid[j,i].transform.localPosition = new(i, j, 0f);
                }
            }
        }
        
        void Update() {
            
        }
    }
}