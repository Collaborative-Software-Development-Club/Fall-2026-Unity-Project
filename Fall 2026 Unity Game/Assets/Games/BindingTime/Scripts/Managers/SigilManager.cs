using System.Collections.Generic;
using UnityEngine;

namespace BindingTime
{
    public class SigilManager
    {
        public const TileType OpenType = TileType.Ground;

        private readonly HashSet<int> activeChannels = new HashSet<int>();

        public bool IsChannelActive(int channel) => activeChannels.Contains(channel);

        public void Refresh(ICollection<Vector2Int> occupied)
        {
            TileManager tiles = TileManager.Instance;

            activeChannels.Clear();
            foreach (Tile sigil in tiles.Sigils)
            {
                if (occupied.Contains(sigil.Coords)) activeChannels.Add(sigil.Channel);
            }
            foreach (Tile sigil in tiles.Sigils) sigil.Powered = activeChannels.Contains(sigil.Channel);

            foreach (Tile figure in tiles.Figures)
            {
                bool powered = activeChannels.Contains(figure.Channel);
                bool open = (powered != figure.Inverted) || occupied.Contains(figure.Coords);
                tiles.SwapTile(figure, open ? OpenType : figure.BaseType);
            }
        }
    }
}