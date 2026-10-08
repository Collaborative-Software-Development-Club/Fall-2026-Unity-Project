using System;
using UnityEngine;

namespace BindingTime
{
    public class TileManager : MonoBehaviour
    {
        public static TileManager Instance { get; private set; }

        [Header("Levels in play order (drag the .txt files here)")]
        [SerializeField] private TextAsset[] levels;

        [SerializeField] private GameObject childPrefab;
        [SerializeField] private bool fitCamera = true;

        [Header("Fallback level, only used if a level slot is empty")]
        [SerializeField] private int widthTiles = 50;
        [SerializeField] private int heightTiles = 30;

        [Header("Optional sprites (leave empty to use the prefab's sprite, tinted)")]
        [SerializeField] private Sprite floorSprite;
        [SerializeField] private Sprite wallSprite;
        [SerializeField] private Sprite entranceSprite;
        [SerializeField] private Sprite exitSprite;

        private Tile[,] tileGrid; // [y, x]

        // Size of the level that is currently loaded
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Vector2Int Entrance { get; private set; }
        public Vector2Int Exit { get; private set; }

        public int CurrentLevel { get; private set; }
        public bool HasNextLevel => levels != null && CurrentLevel + 1 < levels.Length;

        // passes the level index
        public event Action<int> LevelLoaded;

        // Built in Awake so tiles exist before any Start() runs
        void Awake()
        {
            Instance = this;
            LoadLevel(0);
        }

        // ---------- Queries ----------

        public bool InBounds(int x, int y) =>
            x >= 0 && y >= 0 && x < Width && y < Height;

        public Tile GetTile(int x, int y) => InBounds(x, y) ? tileGrid[y, x] : null;

        public bool IsWalkable(int x, int y) => InBounds(x, y) && tileGrid[y, x].IsWalkable;

        // ---------- Level loading ----------

        public void NextLevel()
        {
            if (!HasNextLevel)
            {
                Debug.LogWarning("NextLevel called but there are no more levels.");
                return;
            }
            LoadLevel(CurrentLevel + 1);
        }

        public void LoadLevel(int index)
        {
            ClearTiles();
            CurrentLevel = index;

            TextAsset file = (levels != null && index >= 0 && index < levels.Length) ? levels[index] : null;
            TileType[,] map = file != null ? ParseLevel(file.text) : DefaultLevel();

            Build(map);
            if (fitCamera) FitCamera();

            LevelLoaded?.Invoke(index);
        }

        private void ClearTiles()
        {
            if (tileGrid == null) return;
            foreach (Tile tile in tileGrid)
            {
                if (tile != null) Destroy(tile.gameObject);
            }
            tileGrid = null;
        }

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

        // key:  #  wall    .  floor    S  entrance (spawn    E  exit
        // levels r displayed bottom to top
        private TileType[,] ParseLevel(string text)
        {
            // ignore blank lines at the end of the file
            string[] lines = text.Replace("\r", "").TrimEnd('\n').Split('\n');
            int h = lines.Length;
            int w = 0;
            foreach (var line in lines) w = Mathf.Max(w, line.Length);

            var map = new TileType[h, w];

            for (int row = 0; row < h; row++)
            {
                int y = h - 1 - row; // flip so the first line is the top
                for (int x = 0; x < w; x++)
                {
                    char c = x < lines[row].Length ? lines[row][x] : '#'; // pad short lines with wall js incase
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

            if (!foundEntrance) Debug.LogWarning($"Level {CurrentLevel} has no entrance (S). Player will spawn at (0,0).");
            if (!foundExit) Debug.LogWarning($"Level {CurrentLevel} has no exit (E).");
        }

        // Centers camera && zoom
        private void FitCamera()
        {
            Camera cam = Camera.main;
            if (cam == null || !cam.orthographic) return;

            Vector3 center = transform.TransformPoint(new Vector3((Width - 1) / 2f, (Height - 1) / 2f, 0f));
            cam.transform.position = new Vector3(center.x, center.y, cam.transform.position.z);

            float halfHeight = Height / 2f + 0.5f;
            float halfWidth = (Width / 2f + 0.5f) / cam.aspect;
            cam.orthographicSize = Mathf.Max(halfHeight, halfWidth);
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