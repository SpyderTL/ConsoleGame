﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame
{
	internal static class RpgMap
	{
		internal static void Update()
		{
			var zones = new List<Map.Zone>();

			for (var row = 0; row < RpgGame.Map.Rows.Length; row++)
			{
				var column = 0;

				foreach (var segment in RpgGame.Map.Rows[row].Segments)
				{
					zones.Add(new Map.Zone { Top = row, Left = column, Bottom = row, Right = column + segment.Width - 1, Character = Tiles[segment.Tile].Character, Description = Tiles[segment.Tile].Name });

					column += segment.Width;
				}
			}

			Map.Zones = zones.ToArray();
		}

		internal class Tile
		{
			internal string Name;
			internal char Character;
		}

		internal static Tile[] Tiles = new Tile[]
		{
			new Tile { Character = '.', Name = "Grass" },
			new Tile { Character = '[',  Name = "Castle 1 - Lower Left" },
			new Tile { Character = ']',  Name = "Castle 1 - Lower Right" },
			new Tile { Character = '!',  Name = "Forest - Top Left" } ,
			new Tile { Character = '!',  Name = "Forest - Top" },
			new Tile { Character = '!',  Name = "Forest - Top Right" },
			new Tile { Character = '.',  Name = "Grass - Bottom Right" },
			new Tile { Character = '.',  Name = "Grass - Bottom" },
			new Tile { Character = '.',  Name = "Grass - Bottom Left" },
			new Tile { Character = '[',  Name = "Castle 1 - Top Left" },
			new Tile { Character = ']',  Name = "Castle 1 - Top Right" },
			new Tile { Character = '[',  Name = "Castle 2 - Top Left" },
			new Tile { Character = ']',  Name = "Castle 2 - Top Right" },
			new Tile { Character = 'i',  Name = "Tower of Illusion - Top" },
			new Tile { Character = '`',  Name = "Grotto 1" },
			new Tile { Character = '=',  Name = "Dock - Vertical Left" },

			new Tile { Character = '^',  Name = "Mountain - Top Left" },
			new Tile { Character = '^',  Name = "Mountain - Top" },
			new Tile { Character = '^',  Name = "Mountain - Top Right" },
			new Tile { Character = '!',  Name = "Forest - Left" },
			new Tile { Character = '!',  Name = "Forest - Bottom" },
			new Tile { Character = '!',  Name = "Forest - Right" },
			new Tile { Character = '.',  Name = "Grass - Right" },
			new Tile { Character = '\0',  Name = "Ocean" },
			new Tile { Character = '.',  Name = "Grass - Left" },
			new Tile { Character = '[',  Name = "Castle 1 - Middle Left" },
			new Tile { Character = ']',  Name = "Castle 1 - Middle Right" },
			new Tile { Character = '[',  Name = "Castle 2 - Bottom Left" },
			new Tile { Character = ']',  Name = "Castle 2 - Bottom Right" },
			new Tile { Character = 'O',  Name = "Tower of Illusion - Bottom" },
			new Tile { Character = '.',  Name = "Tower of Illusion - Shadow" },
			new Tile { Character = '=',  Name = "Dock - Vertical Right" },

			new Tile { Character = '^',  Name = "Mountain - Left" },
			new Tile { Character = '^',  Name = "Mountain" },
			new Tile { Character = '^',  Name = "Mountain - Right" },
			new Tile { Character = '!',  Name = "Forest - Bottom Left" },
			new Tile { Character = '!',  Name = "Forest - Bottom" },
			new Tile { Character = '!',  Name = "Forest - Bottom Right" },
			new Tile { Character = '.',  Name = "Grass - Top Right" },
			new Tile { Character = '.',  Name = "Grass - Top" },
			new Tile { Character = '.',  Name = "Grass - Top Left" },
			new Tile { Character = '[',  Name = "Castle 2 - Bottom Left" },
			new Tile { Character = ']',  Name = "Castle 2 - Bottom Right" },
			new Tile { Character = '`',  Name = "Grotto 2" },
			new Tile { Character = '#',  Name = "Village Fence - Top Left" },
			new Tile { Character = '#',  Name = "Village Fence - Top" },
			new Tile { Character = '#',  Name = "Village Fence - Top Right" },
			new Tile { Character = '`',  Name = "Grotto 3" },

			new Tile { Character = '^',  Name = "Mountain - Bottom Left" },
			new Tile { Character = '^',  Name = "Mountain - Bottom" },
			new Tile { Character = '`',  Name = "Grotto 4" },
			new Tile { Character = '^',  Name = "Mountain - Bottom Right" },
			new Tile { Character = '`',  Name = "Grotto 5" },
			new Tile { Character = '`',  Name = "Grotto 6" },
			new Tile { Character = '~',  Name = "Desert" },
			new Tile { Character = '~',  Name = "Desert" },
			new Tile { Character = '[',  Name = "Castle 2 - Bottom Left" },
			new Tile { Character = ']',  Name = "Castle 2 - Bottom Right" },
			new Tile { Character = '`',  Name = "Grotto 7" },
			new Tile { Character = '#',  Name = "Village Fence - Top Left Exterior" },
			new Tile { Character = '#',  Name = "Village Fence - Top Left Interior" },
			new Tile { Character = '.',  Name = "Village Area (gray)" },
			new Tile { Character = '#',  Name = "Village Fence - Top Right Interior" },
			new Tile { Character = '#',  Name = "Village Fence - Top Right Exterior" },

			new Tile { Character = '\0',  Name = "River - Top Left" },
			new Tile { Character = '\0',  Name = "River - Top Right" },
			new Tile { Character = '~',  Name = "Desert - Top Left" },
			new Tile { Character = '~',  Name = "Desert - Top Right" },
			new Tile { Character = '\0',  Name = "River" },
			new Tile { Character = '~',  Name = "Desert" },
			new Tile { Character = '\0',  Name = "Waterfall" },
			new Tile { Character = '[',  Name = "Castle Ruins - Top Left" },
			new Tile { Character = ']',  Name = "Castle Ruins - Top Right" },
			new Tile { Character = '+',  Name = "Village 1" },
			new Tile { Character = '+',  Name = "Village 2" },
			new Tile { Character = '#',  Name = "Village Fence - Middle Left" },
			new Tile { Character = '+',  Name = "Village 3" },
			new Tile { Character = '+',  Name = "Village 4" },
			new Tile { Character = '+',  Name = "Village 5" },
			new Tile { Character = '#',  Name = "Village Fence - Middle Right" },

			new Tile { Character = '\0',  Name = "River - Bottom Left" },
			new Tile { Character = '\0',  Name = "River - Bottom Right" },
			new Tile { Character = '~',  Name = "Desert - Bottom Left" },
			new Tile { Character = '~',  Name = "Desert - Bottom Right" },
			new Tile { Character = ',',  Name = "Tall Grass" },
			new Tile { Character = '_',  Name = "Swamp" },
			new Tile { Character = '[',  Name = "Castle Ruins - Bottom Left" },
			new Tile { Character = '[',  Name = "Castle Ruins - Bottom Middle Left" },
			new Tile { Character = ']',  Name = "Castle Ruins - Bottom Middle Right" },
			new Tile { Character = ']',  Name = "Castle Ruins - Bottom Right" },
			new Tile { Character = '+',  Name = "Village 6" },
			new Tile { Character = '#',  Name = "Village Fence - Middle Left 2" },
			new Tile { Character = '#',  Name = "Village Fence - Bottom Left" },
			new Tile { Character = '+',  Name = "Village 7" },
			new Tile { Character = '#',  Name = "Village Fence - Bottom Right" },
			new Tile { Character = '#',  Name = "Village Fence - Middle Right 2" },

			new Tile { Character = ',',  Name = "Tall Grass - Top Left" },
			new Tile { Character = ',',  Name = "Tall Grass - Top Right" },
			new Tile { Character = '_',  Name = "Swamp - Top Left" },
			new Tile { Character = '_',  Name = "Swamp - Top Right" },
			new Tile { Character = '^',  Name = "Volcano - Top Left" },
			new Tile { Character = '^',  Name = "Volcano - Top Right" },
			new Tile { Character = 'O',  Name = "Hole 1" },
			new Tile { Character = 'O',  Name = "Hole 2" },
			new Tile { Character = 'O',  Name = "Hole 3" },
			new Tile { Character = 'O',  Name = "Hole 4" },
			new Tile { Character = 'O',  Name = "Hole 5" },
			new Tile { Character = '#',  Name = "Village Fence - Middle Left 3" },
			new Tile { Character = 'O',  Name = "Hole 6" },
			new Tile { Character = '+',  Name = "Village 8" },
			new Tile { Character = 'O',  Name = "Hole 7" },
			new Tile { Character = '#',  Name = "Village Fence - Middle Right 3" },

			new Tile { Character = ',',  Name = "Tall Grass - Bottom Left" },
			new Tile { Character = ',',  Name = "Tall Grass - Bottom Right" },
			new Tile { Character = '_',  Name = "Swamp - Bottom Left" },
			new Tile { Character = '_',  Name = "Swamp - Bottom Right" },
			new Tile { Character = '^',  Name = "Volcano - Bottom Left" },
			new Tile { Character = '^',  Name = "Volcano - Bottom Right" },
			new Tile { Character = '.',  Name = "Grass" },
			new Tile { Character = '=',  Name = "Dock - Bottom Right" },
			new Tile { Character = '=',  Name = "Dock - Bottom" },
			new Tile { Character = '=',  Name = "Dock - Bottom Left" },
			new Tile { Character = '=',  Name = "Dock - Top Left" },
			new Tile { Character = '#',  Name = "Village Fence - Bottom Left" },
			new Tile { Character = '#',  Name = "Village Wall 1" },
			new Tile { Character = '#',  Name = "Village Wall 2" },
			new Tile { Character = '#',  Name = "Village Wall 3" },
			new Tile { Character = '#',  Name = "Village Fence - Bottom Right" },
			//new Tile { Character = '\0',  Name = "Ocean" },
			//new Tile { Character = '\0',  Name = "Unknown" },
		};
	}
}
