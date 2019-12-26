using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame
{
	internal static class RpgMap
	{
		internal static Tile[] Tiles = new Tile[128];

		internal static void Enable()
		{
			RpgGame.PartyMap.PositionChanged += PartyMap_PositionChanged;

			Party.X = RpgGame.PartyMap.X;
			Party.Y = RpgGame.PartyMap.Y;
		}

		internal static void Disable()
		{
			RpgGame.PartyMap.PositionChanged -= PartyMap_PositionChanged;
		}

		private static void PartyMap_PositionChanged()
		{
			Party.X = RpgGame.PartyMap.X;
			Party.Y = RpgGame.PartyMap.Y;

			MapScreen.Draw();
		}

		internal static void Load()
		{
			for (var tile = 0; tile < Tiles.Length; tile++)
			{
				switch (RpgGame.Map.Tiles[tile].TileType)
				{
					default:
						if (RpgGame.Map.Tiles[tile].Blocked)
						{
							Tiles[tile].Name = "Wall";
							Tiles[tile].Character = '#';
						}
						else
						{
							Tiles[tile].Name = "Open";
							Tiles[tile].Character = '.';
						}
						break;
				}
			}

			var zones = new List<Map.Zone>();

			for (var row = 0; row < RpgGame.PartyMap.Rows.Length; row++)
			{
				foreach (var segment in RpgGame.PartyMap.Rows[row])
				{
					zones.Add(new Map.Zone { Top = row, Left = segment.Left, Bottom = row, Right = segment.Right, Character = Tiles[segment.Tile].Character, Description = Tiles[segment.Tile].Name });
				}
			}

			Map.Zones = zones.ToArray();
		}

		internal struct Tile
		{
			internal string Name;
			internal char Character;
		}
	}
}
