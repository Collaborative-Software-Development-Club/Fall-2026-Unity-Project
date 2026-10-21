using UnityEngine;

namespace BindingTime
{
    public enum TileType { Ground, Wall, Origin, Moss, Figure, Water, Exit, Sigil } //statue = figure (this is so we can use S for sigil) 

    [RequireComponent(typeof(SpriteRenderer))]
    public class Tile : MonoBehaviour
    {
        private SpriteRenderer sr;
        private bool powered;

        public TileType Type { get; private set; }

        public TileType BaseType { get; private set; }

        public Vector2Int Coords { get; private set; }

        public bool IsWalkable => Type != TileType.Wall && Type != TileType.Figure;

        public bool Powered
        {
            get => powered;
            set
            {
                if (powered == value) return;
                powered = value;
                ApplyTint();
            }
        }

        public void Setup(TileType type, Vector2Int coords, Sprite spriteOverride = null)
        {
            BaseType = type;
            Type = type;
            Coords = coords;
            powered = false;

            if (spriteOverride != null) SpriteRend.sprite = spriteOverride;
            Rename();
            ApplyTint();
        }

        public void SwapTo(TileType type, Sprite spriteOverride = null)
        {
            Type = type;
            if (spriteOverride != null) SpriteRend.sprite = spriteOverride;
            Rename();
            ApplyTint();
        }

        private SpriteRenderer SpriteRend => sr != null ? sr : (sr = GetComponent<SpriteRenderer>());

        private void Rename()
        {
            gameObject.name = $"Tile {Type} ({Coords.x},{Coords.y})";
        }

        // Placeholder tint so the types are visible before you have real art.
        // If you assign a sprite, set the color back to white if you don't want it tinted.
        private void ApplyTint()
        {
            SpriteRend.color = Type switch
            {
                TileType.Wall => new Color(0.25f, 0.25f, 0.30f),
                TileType.Origin => new Color(0.30f, 0.85f, 0.40f),
                TileType.Exit => new Color(0.95f, 0.80f, 0.20f),
                TileType.Figure => new Color(0.60f, 0.60f, 0.65f),
                TileType.Sigil => powered ? new Color(0.85f, 0.60f, 1.00f) : new Color(0.45f, 0.25f, 0.70f),
                _ => Color.black,
            };
        }
    }
}