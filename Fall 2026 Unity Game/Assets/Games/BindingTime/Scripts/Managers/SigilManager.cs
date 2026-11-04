using System.Collections.Generic;
using UnityEngine;

namespace BindingTime
{
	public class SigilManager
	{
		public const TileType OpenType = TileType.Ground;

		private readonly HashSet<int> activeChannels = new HashSet<int>();
		private readonly HashSet<Vector2Int> forcedOpen = new HashSet<Vector2Int>();

		public bool IsChannelActive(int channel) => activeChannels.Contains(channel);
		public bool IsForcedOpen(Vector2Int coords) => forcedOpen.Contains(coords);
		public void Refresh(ICollection<Vector2Int> occupied)
		{
			TileManager tiles = TileManager.Instance;

			activeChannels.Clear();
			forcedOpen.Clear();
			foreach (Tile sigil in tiles.Sigils)
			{
				if (occupied.Contains(sigil.Coords)) activeChannels.Add(sigil.Channel);
			}
			foreach (Tile sigil in tiles.Sigils) sigil.Powered = activeChannels.Contains(sigil.Channel);

			foreach (Tile figure in tiles.Figures)
			{
				bool powered = activeChannels.Contains(figure.Channel);
				bool natural = powered != figure.Inverted;
				bool open = natural || occupied.Contains(figure.Coords);
				if (!natural && open) forcedOpen.Add(figure.Coords);
				tiles.SwapTile(figure, open ? OpenType : figure.BaseType);
			}
		}
	}
}
