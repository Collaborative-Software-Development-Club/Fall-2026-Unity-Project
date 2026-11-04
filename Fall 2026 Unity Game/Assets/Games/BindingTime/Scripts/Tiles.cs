using UnityEngine;

namespace BindingTime
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Tile : MonoBehaviour
    {
        private static readonly Color[] ChannelColors =
        {
            new Color(0.70f, 0.70f, 0.75f), // 0 grey (S / F)
            new Color(0.95f, 0.35f, 0.35f), // 1 red
            new Color(0.35f, 0.65f, 1.00f), // 2 blue
            new Color(0.40f, 0.90f, 0.45f), // 3 green
            new Color(1.00f, 0.80f, 0.25f), // 4 yellow
            new Color(0.85f, 0.45f, 1.00f), // 5 purple
            new Color(1.00f, 0.60f, 0.25f), // 6 orange
            new Color(0.30f, 0.90f, 0.90f), // 7 cyan
            new Color(1.00f, 0.55f, 0.80f), // 8 pink
            new Color(0.90f, 0.90f, 0.90f), // 9 white
        };

        private SpriteRenderer sr;
        private bool powered;

        public TileType Type { get; private set; }
        public TileType BaseType { get; private set; }
        public Vector2Int Coords { get; private set; }
        public int Channel { get; private set; }
        public bool Inverted { get; private set; }

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

        private SpriteRenderer SpriteRend => sr != null ? sr : (sr = GetComponent<SpriteRenderer>());
        private Color ChannelColor => ChannelColors[Mathf.Clamp(Channel, 0, ChannelColors.Length - 1)];
        private bool IsLinked => BaseType == TileType.Sigil || BaseType == TileType.Figure;

        public void Setup(LevelCell cell, Vector2Int coords, Sprite spriteOverride = null)
        {
            BaseType = cell.Type;
            Channel = cell.Channel;
            Inverted = cell.Inverted;
            Coords = coords;
            powered = false;
            SwapTo(cell.Type, spriteOverride);
        }

        public void SwapTo(TileType type, Sprite spriteOverride = null)
        {
            Type = type;
            if (spriteOverride != null) SpriteRend.sprite = spriteOverride;
            Rename();
            ApplyTint();
        }

        private void Rename()
        {
            string link = IsLinked ? $" ch{Channel}{(Inverted ? " inv" : "")}" : "";
            gameObject.name = $"Tile {Type}{link} ({Coords.x},{Coords.y})";
        }

        private void ApplyTint()
        {
            SpriteRend.color = BaseType == TileType.Figure ? StatueColor() : TileColor();
        }

        // an open statue is swapped to Ground, so BaseType decides it's a statue
        private Color StatueColor()
        {
            if (Type != TileType.Figure) return ChannelColor * 0.3f;
            return Color.Lerp(new Color(0.45f, 0.45f, 0.5f), ChannelColor, Inverted ? 0.85f : 0.5f);
        }

        private Color TileColor() => Type switch
        {
            TileType.Wall => new Color(0.25f, 0.25f, 0.30f),
            TileType.Origin => new Color(0.30f, 0.85f, 0.40f),
            TileType.Exit => new Color(0.95f, 0.80f, 0.20f),
            TileType.Sigil => powered ? ChannelColor : ChannelColor * 0.45f,
            _ => Color.black,
        };
    }
}
