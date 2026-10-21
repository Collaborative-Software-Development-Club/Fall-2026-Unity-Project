using System.Collections.Generic;
using UnityEngine;

namespace BindingTime
{
	public class SigilManager
	{
		public const TileType OpenType = TileType.Ground;

		public bool Active { get; private set; }

		public void Refresh(ICollection<Vector2Int> occupied)
		{
			TileManager tiles = TileManager.Instance;

			Active = false;
			foreach (Tile sigil in tiles.Sigils)
			{
				if (occupied.Contains(sigil.Coords)) { Active = true; break; }
			}
			foreach (Tile sigil in tiles.Sigils) sigil.Powered = Active;

			foreach (Tile figure in tiles.Figures)
			{
				bool open = Active || occupied.Contains(figure.Coords);
				tiles.SwapTile(figure, open ? OpenType : figure.BaseType);
			}
		}
	}
}