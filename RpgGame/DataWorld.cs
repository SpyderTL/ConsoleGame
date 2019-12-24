﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame
{
	public static class DataWorld
	{
		public static void Load()
		{
			using (var reader = Data.Reader())
			{
				// Load Tiles
				reader.BaseStream.Position = Data.Address(0, 0x8000);

				for (var tile = 0; tile < 128; tile++)
				{
					var value = reader.ReadByte();
					var value2 = reader.ReadByte();

					World.Tiles[tile] = new World.Tile
					{
						Blocked = (value & 0x01) == 0x01,
						TileType = (World.TileType)((value >> 1) & 0x0f),
						TeleportType = (World.TeleportType)(value >> 6),
						Battle = (value & 0x20) == 0x20,
						Value = value2
					};
				}

				// Load Segments
				reader.BaseStream.Position = Data.Address(1, 0x8000);

				World.Rows = new World.Row[256];

				var rows = new int[256];

				for (var row = 0; row < 256; row++)
					rows[row] = reader.ReadUInt16();

				for (var row = 0; row < 256; row++)
				{
					reader.BaseStream.Position = Data.Address(1, rows[row]);

					var segments = new List<World.Segment>();

					while (true)
					{
						var value = reader.ReadByte();

						if (value == 0xff)
							break;

						var count = 1;

						if ((value & 0x80) == 0x80)
						{
							value &= 0x7f;

							count = reader.ReadByte();

							if (count == 0)
								count = 256;
						}

						segments.Add(new World.Segment { Tile = value, Count = count });
					}

					World.Rows[row].Segments = segments.ToArray();
				}
			}
		}
	}
}