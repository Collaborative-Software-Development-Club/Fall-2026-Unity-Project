using UnityEngine;

namespace BindingTime
{
    public enum TileType { Ground, Wall, Origin, Moss, Figure, Water, Exit, Sigil } //statue = figure (this is so we can use S for sigil) 

    // One cell of a parsed level. Channel links sigils to statues:
    // a sigil on channel N powers every statue on channel N. Channel 0 is the old S / F pair.
    public struct LevelCell
    {
        public TileType Type;
        public int Channel;
        public bool Inverted; // inverted statues are OPEN by default and CLOSE while their channel is powered

        public LevelCell(TileType type, int channel = 0, bool inverted = false)
        {
            Type = type;
            Channel = channel;
            Inverted = inverted;
        }
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class Tile : MonoBehaviour
    {
        private static readonly Color[] ChannelColors =
        {
            new Color(0.70f, 0.70f, 0.75f), // 0 (legacy S / F)
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

        public void Setup(LevelCell cell, Vector2Int coords, Sprite spriteOverride = null)
        {
            BaseType = cell.Type;
            Type = cell.Type;
            Channel = cell.Channel;
            Inverted = cell.Inverted;
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

        private Color ChannelColor => ChannelColors[Mathf.Clamp(Channel, 0, ChannelColors.Length - 1)];

        private void Rename()
        {
            string link = (BaseType == TileType.Sigil || BaseType == TileType.Figure) ? $" ch{Channel}{(Inverted ? " inv" : "")}" : "";
            gameObject.name = $"Tile {Type}{link} ({Coords.x},{Coords.y})";
        }

        // Placeholder tint so the types are visible before you have real art.
        // If you assign a sprite, set the color back to white if you don't want it tinted.
        private void ApplyTint()
        {
            // A statue that is currently open turns into Ground, so check BaseType too.
            if (BaseType == TileType.Figure)
            {
                if (Type == TileType.Figure)
                {
                    // closed: grey stone washed with its channel colour (inverted ones are brighter)
                    SpriteRend.color = Color.Lerp(new Color(0.45f, 0.45f, 0.5f), ChannelColor, Inverted ? 0.85f : 0.5f);
                }
                else
                {
                    // open: dark floor with a faint channel colour so you can still see where it is
                    SpriteRend.color = ChannelColor * 0.3f;
                }
                return;
            }

            SpriteRend.color = Type switch
            {
                TileType.Wall => new Color(0.25f, 0.25f, 0.30f),
                TileType.Origin => new Color(0.30f, 0.85f, 0.40f),
                TileType.Exit => new Color(0.95f, 0.80f, 0.20f),
                TileType.Sigil => powered ? ChannelColor : ChannelColor * 0.45f,
                _ => Color.black,
            };
        }
    }
}