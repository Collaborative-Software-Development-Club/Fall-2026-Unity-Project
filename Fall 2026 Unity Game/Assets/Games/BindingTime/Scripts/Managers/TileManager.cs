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

        public bool InBounds(Vector2Int c) => c.x >= 0 && c.y >= 0 && c.x < Width && c.y < Height;

        public Tile GetTile(Vector2Int c) => InBounds(c) ? tileGrid[c.y, c.x] : null;

        public bool IsWalkable(Vector2Int c) => InBounds(c) && tileGrid[c.y, c.x].IsWalkable;

        public bool IsSigil(Vector2Int c)
        {
            Tile tile = GetTile(c);
            return tile != null && tile.BaseType == TileType.Sigil;
        }

        public void SwapTile(Tile tile, TileType type)
        {
            if (tile.Type != type) tile.SwapTo(type, SpriteFor(type));
        }

        public void Build(LevelCell[,] map)
        {
            Clear();

            Height = map.GetLength(0);
            Width = map.GetLength(1);
            tileGrid = new Tile[Height, Width];
            Origin = Vector2Int.zero;
            Exit = Vector2Int.zero;
            bool foundOrigin = false, foundExit = false;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var coords = new Vector2Int(x, y);
                    LevelCell cell = map[y, x];
                    Tile tile = CreateTile(cell, coords);
                    tileGrid[y, x] = tile;

                    switch (cell.Type)
                    {
                        case TileType.Origin: Origin = coords; foundOrigin = true; break;
                        case TileType.Exit: Exit = coords; foundExit = true; break;
                        case TileType.Sigil: sigils.Add(tile); break;
                        case TileType.Figure: figures.Add(tile); break;
                    }
                }
            }

            if (!foundOrigin) Debug.LogWarning("Map has no origin (O). Player will spawn at (0,0).");
            if (!foundExit) Debug.LogWarning("Map has no exit (E).");
        }

        private Tile CreateTile(LevelCell cell, Vector2Int coords)
        {
            GameObject go = Instantiate(childPrefab, transform);
            go.transform.localPosition = new Vector3(coords.x, coords.y, 0f);

            Tile tile = go.GetComponent<Tile>();
            if (tile == null) tile = go.AddComponent<Tile>();
            tile.Setup(cell, coords, SpriteFor(cell.Type));
            return tile;
        }

        private void Clear()
        {
            sigils.Clear();
            figures.Clear();
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
