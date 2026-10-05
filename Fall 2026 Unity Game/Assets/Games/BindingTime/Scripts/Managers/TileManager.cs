using UnityEngine;

namespace BindingTime
{
    public class TileManager : MonoBehaviour
    {
        public static TileManager Instance { get; private set; }

        [SerializeField] private int widthTiles = 50;  // arbitrary
        [SerializeField] private int heightTiles = 30; // abritrary i cant spell
        [SerializeField] private GameObject childPrefab;

        [Header("Optional: text file describing the level (see ParseLevel for legend)")]
        [SerializeField] private TextAsset levelFile;

        [Header("Optional sprites (leave empty to use the prefab's sprite, tinted)")]
        [SerializeField] private Sprite floorSprite;
        [SerializeField] private Sprite wallSprite;
        [SerializeField] private Sprite entranceSprite;
        [SerializeField] private Sprite exitSprite;

        private Tile[,] tileGrid; // [y, x]

        public Vector2Int Entrance { get; private set; }
        public Vector2Int Exit { get; private set; }

        // Built in Awake so tiles exist before any Start() (e.g. Player) runs.
        void Awake()
        {
            Instance = this;
            TileType[,] map = levelFile != null ? ParseLevel(levelFile.text) : DefaultLevel();
            Build(map);
        }

        // ---------- Queries ----------

        public bool InBounds(int x, int y) =>
            x >= 0 && y >= 0 && x < widthTiles && y < heightTiles;

        public Tile GetTile(int x, int y) => InBounds(x, y) ? tileGrid[y, x] : null;

        public bool IsWalkable(int x, int y) => InBounds(x, y) && tileGrid[y, x].IsWalkable;

        // ---------- Level creation ----------

        private TileType[,] DefaultLevel()
        {
            var map = new TileType[heightTiles, widthTiles];
            for (int y = 0; y < heightTiles; y++)
            {
                for (int x = 0; x < widthTiles; x++)
                {
                    bool border = x == 0 || y == 0 || x == widthTiles - 1 || y == heightTiles - 1;
                    map[y, x] = border ? TileType.Wall : TileType.Floor;
                }
            }
            map[1, 1] = TileType.Entrance;
            map[heightTiles - 2, widthTiles - 2] = TileType.Exit;
            return map;
        }

        // key:  #  wall    .  floor    S  entrance (spawn)    E  exit
        // levels r displayed bottom to top
        private TileType[,] ParseLevel(string text)
        {
            string[] lines = text.Replace("\r", "").Split('\n');
            int h = lines.Length;
            int w = 0;
            foreach (var line in lines) w = Mathf.Max(w, line.Length);

            widthTiles = w;
            heightTiles = h;
            var map = new TileType[h, w];

            for (int row = 0; row < h; row++)
            {
                int y = h - 1 - row; // flip so the first line is the top
                for (int x = 0; x < w; x++)
                {
                    char c = x < lines[row].Length ? lines[row][x] : '#'; // pad short lines with wall
                    map[y, x] = c switch
                    {
                        '#' => TileType.Wall,
                        'S' => TileType.Entrance,
                        'E' => TileType.Exit,
                        _ => TileType.Floor,
                    };
                }
            }
            return map;
        }

        private void Build(TileType[,] map)
        {
            tileGrid = new Tile[heightTiles, widthTiles];
            bool foundEntrance = false, foundExit = false;

            for (int y = 0; y < heightTiles; y++)
            {
                for (int x = 0; x < widthTiles; x++)
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

            if (!foundEntrance) Debug.LogWarning("Level has no entrance (S). Player will spawn at (0,0).");
            if (!foundExit) Debug.LogWarning("Level has no exit (E).");
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