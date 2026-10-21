using System.Collections.Generic;
using UnityEngine;

namespace BindingTime
{
    public class TileManager : MonoBehaviour
    {
        public static TileManager Instance { get; private set; }

        [SerializeField] private GameObject childPrefab;

        [Header("Optional sprites (leave empty to use the prefab's sprite, tinted)")]
        [SerializeField] private Sprite floorSprite;
        [SerializeField] private Sprite wallSprite;
        [SerializeField] private Sprite originSprite;
        [SerializeField] private Sprite exitSprite;

        private Tile[,] tileGrid; // [y, x]
        private readonly List<Tile> sigils = new List<Tile>();
        private readonly List<Tile> figures = new List<Tile>();
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Vector2Int Origin { get; private set; }
        public Vector2Int Exit { get; private set; }

        public IReadOnlyList<Tile> Sigils => sigils;
        public IReadOnlyList<Tile> Figures => figures;

        void Awake()
        {
            Instance = this;
        }

        // ---------- Queries ----------

        public bool InBounds(int x, int y) =>
            x >= 0 && y >= 0 && x < Width && y < Height;

        public Tile GetTile(int x, int y) => InBounds(x, y) ? tileGrid[y, x] : null;

        public bool IsWalkable(int x, int y) => InBounds(x, y) && tileGrid[y, x].IsWalkable;

        public void SwapTile(Tile tile, TileType type)
        {
            if (tile.Type != type) tile.SwapTo(type, SpriteFor(type));
        }

        // ---------- Building ----------

        public void Build(LevelCell[,] map)
        {
            Clear();

            Height = map.GetLength(0);
            Width = map.GetLength(1);
            tileGrid = new Tile[Height, Width];
            sigils.Clear();
            figures.Clear();
            Origin = Vector2Int.zero;
            Exit = Vector2Int.zero;
            bool foundOrigin = false, foundExit = false;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    LevelCell cell = map[y, x];
                    TileType type = cell.Type;
                    if (type == TileType.Origin) { Origin = new Vector2Int(x, y); foundOrigin = true; }
                    if (type == TileType.Exit) { Exit = new Vector2Int(x, y); foundExit = true; }

                    GameObject go = Instantiate(childPrefab, transform);
                    go.transform.localPosition = new Vector3(x, y, 0f);

                    Tile tile = go.GetComponent<Tile>();
                    if (tile == null) tile = go.AddComponent<Tile>();
                    tile.Setup(cell, new Vector2Int(x, y), SpriteFor(type));
                    tileGrid[y, x] = tile;
                    if (type == TileType.Sigil) sigils.Add(tile);
                    if (type == TileType.Figure) figures.Add(tile);
                }
            }

            if (!foundOrigin) Debug.LogWarning("Map has no origin (O). Player will spawn at (0,0).");
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
            TileType.Origin => originSprite,
            TileType.Exit => exitSprite,
            _ => floorSprite,
        };
    }
}
