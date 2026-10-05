using UnityEngine;

namespace BindingTime
{
    public enum TileType { Floor, Wall, Entrance, Exit }

    [RequireComponent(typeof(SpriteRenderer))]
    public class Tile : MonoBehaviour
    {
        public TileType Type { get; private set; }
        public Vector2Int Coords { get; private set; }

        // Walls are the only thing you can't walk on.
        public bool IsWalkable => Type != TileType.Wall;

        public void Setup(TileType type, Vector2Int coords, Sprite spriteOverride = null)
        {
            Type = type;
            Coords = coords;
            gameObject.name = $"Tile {type} ({coords.x},{coords.y})";

            var sr = GetComponent<SpriteRenderer>();
            if (spriteOverride != null) sr.sprite = spriteOverride;

            // Placeholder tint so the types are visible before you have real art.
            // If you assign a sprite, set the color back to white if you don't want it tinted.
            sr.color = type switch
            {
                TileType.Wall => new Color(0.25f, 0.25f, 0.30f),
                TileType.Entrance => new Color(0.30f, 0.85f, 0.40f),
                TileType.Exit => new Color(0.95f, 0.80f, 0.20f),
                _ => Color.black,
            };
        }
    }
}