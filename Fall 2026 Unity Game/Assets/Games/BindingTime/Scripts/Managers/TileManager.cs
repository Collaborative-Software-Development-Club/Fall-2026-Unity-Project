using UnityEngine;

namespace BindingTime
{
    // Builds the tile grid from a map and answers questions about it.
    // It doesn't know about levels: LevelManager decides which map to build.
    public class TileManager : MonoBehaviour
    {
        public static TileManager Instance { get; private set; }

        [SerializeField] private GameObject childPrefab;

        [Header("Optional sprites (leave empty to use the prefab's sprite, tinted)")]
        [SerializeField] private Sprite floorSprite;
        [SerializeField] private Sprite wallSprite;
        [SerializeField] private Sprite entranceSprite;
        [SerializeField] private Sprite exitSprite;

        private Tile[,] tileGrid; // [y, x]
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Vector2Int Entrance { get; private set; }
        public Vector2Int Exit { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        // ---------- Queries ----------

        public bool InBounds(int x, int y) =>
            x >= 0 && y >= 0 && x < Width && y < Height;

        public Tile GetTile(int x, int y) => InBounds(x, y) ? tileGrid[y, x] : null;

        public bool IsWalkable(int x, int y) => InBounds(x, y) && tileGrid[y, x].IsWalkable;

        // ---------- Building ----------

        // replaces whatever grid exists with one built from map ([y, x]).
        public void Build(TileType[,] map)
        {
            Clear();

            Height = map.GetLength(0);
            Width = map.GetLength(1);
            tileGrid = new Tile[Height, Width];
            Entrance = Vector2Int.zero;
            Exit = Vector2Int.zero;
            bool foundEntrance = false, foundExit = false;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    TileType type = map[y, x];
                    if (type == TileType.Entrance) { Entrance = new Vector2Int(x, y); foundEntrance = true; }
                    if (type == TileType.Exit) { Exit = new Vector2Int(x, y); foundExit = true; }

                    GameObject go = Instantiate(childPrefab, transform);
                    go.transform.localPosition = new Vector3(x, y, 0f);

                    Tile tile = go.GetComponent<Tile>();
                    if (tile == null) tile = go.AddComponent<Tile>();
                    tile.Setup(type, new Vector2Int(x, y), SpriteFor(type));
                    tileGrid[y, x] = tile;
                }
            }

            if (!foundEntrance) Debug.LogWarning("Map has no entrance (S). Player will spawn at (0,0).");
            if (!foundExit) Debug.LogWarning("Map has no exit (E).");
        }

        private void Clear()
        {
            if (tileGrid == null) return;
            foreach (Tile tile in tileGrid)
            {
                if (tile != null) Destroy(tile.gameObject);
            }
            tileGrid = null;
        }

        private Sprite SpriteFor(TileType type) => type switch
        {
            TileType.Wall => wallSprite,
            TileType.Entrance => entranceSprite,
            TileType.Exit => exitSprite,
            _ => floorSprite,
        };
    }
}