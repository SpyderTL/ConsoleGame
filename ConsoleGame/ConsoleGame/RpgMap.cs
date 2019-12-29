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
			RpgGame.PartyMap.MapChanged += PartyMap_MapChanged;
			RpgGame.PartyMap.MapExited += PartyMap_MapExited;
			RpgGame.PartyMap.TreasureFound += PartyMap_TreasureFound;
			RpgGame.PartyMap.ObjectResponded += PartyMap_ObjectResponded; ;

			Party.X = RpgGame.PartyMap.X;
			Party.Y = RpgGame.PartyMap.Y;
		}

		private static void PartyMap_ObjectResponded(int objectId, int dialog)
		{
			var lines = RpgGame.Map.Dialogs[dialog].Split("[New Line]");

			Screen.FillRectangle(' ', 5, 5, 6 + lines.Length, 34);

			for(var line = 0; line < lines.Length; line++)
				Screen.DrawString(lines[line], 8, 6 + line);

			Screen.Update();
		}

		private static void PartyMap_TreasureFound(int item)
		{
			Screen.FillRectangle(' ', 10, 0, 15, 39);
			Screen.DrawString("Item Found", 10, 12);
			Screen.DrawString(RpgGame.Map.Items[item].Name, 16, 14);
			Screen.Update();
		}

		private static void PartyMap_MapExited()
		{
			Game.Mode = Game.GameMode.World;

			MapScreen.Hide();
		}

		private static void PartyMap_MapChanged()
		{
			Game.Mode = Game.GameMode.Map;

			MapScreen.Hide();
		}

		internal static void Disable()
		{
			RpgGame.PartyMap.PositionChanged -= PartyMap_PositionChanged;
			RpgGame.PartyMap.MapChanged -= PartyMap_MapChanged;
			RpgGame.PartyMap.MapExited -= PartyMap_MapExited;
			RpgGame.PartyMap.TreasureFound -= PartyMap_TreasureFound;
		}

		private static void PartyMap_PositionChanged()
		{
			Party.X = RpgGame.PartyMap.X;
			Party.Y = RpgGame.PartyMap.Y;

			MapScreen.Draw();
		}

		internal static void Load()
		{
			Map.Width = 64;
			Map.Height = 64;

			for (var tile = 0; tile < Tiles.Length; tile++)
			{
				switch (RpgGame.Map.Tiles[tile].TeleportType)
				{
					case RpgGame.Map.TeleportType.Normal:
						Tiles[tile].Name = "Stairs";
						Tiles[tile].Character = '/';
						break;

					case RpgGame.Map.TeleportType.Warp:
						Tiles[tile].Name = "Stairs";
						Tiles[tile].Character = '\\';
						break;

					case RpgGame.Map.TeleportType.Exit:
						Tiles[tile].Name = "Exit";
						Tiles[tile].Character = 'O';
						break;

					default:
						switch (RpgGame.Map.Tiles[tile].TileType)
						{
							case RpgGame.Map.TileType.Door:
								Tiles[tile].Name = "Door";
								Tiles[tile].Character = ']';
								break;

							case RpgGame.Map.TileType.Locked:
								Tiles[tile].Name = "Locked Door";
								Tiles[tile].Character = ']';
								break;

							case RpgGame.Map.TileType.Treasure:
								Tiles[tile].Name = "Chest";
								Tiles[tile].Character = '$';
								break;

							default:
								if (RpgGame.Map.Tiles[tile].Blocked)
								{
									Tiles[tile].Name = "Wall";
									Tiles[tile].Character = '#';
								}
								else
								{
									Tiles[tile].Name = string.Empty;
									Tiles[tile].Character = '.';
								}
								break;
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
