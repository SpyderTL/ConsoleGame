using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame
{
	public static class DataMap
	{
		public static void Load(int map)
		{
			using (var reader = Data.Reader())
			{
				// Find Tileset
				reader.BaseStream.Position = Data.Address(0, 0xACC0 + map);

				var tileset = reader.ReadByte();

				// Load Tiles
				reader.BaseStream.Position = Data.Address(0, 0x8800 + (tileset * 256));

				for (var tile = 0; tile < 128; tile++)
				{
					var value = reader.ReadByte();
					var value2 = reader.ReadByte();

					Map.Tiles[tile] = new Map.Tile
					{
						Blocked = (value & 0x01) == 0x01,
						TileType = (Map.TileType)((value >> 1) & 0x0f),
						TeleportType = (Map.TeleportType)(value >> 6),
						Battle = (value & 0x20) == 0x20,
						Value = value2
					};
				}

				// Load Teleports
				reader.BaseStream.Position = Data.Address(0, 0xAD80);

				for (var teleport = 0; teleport < Map.Teleports.Length; teleport++)
					Map.Teleports[teleport].Map = reader.ReadByte();

				reader.BaseStream.Position = Data.Address(0, 0xAD00);

				for (var teleport = 0; teleport < Map.Teleports.Length; teleport++)
					Map.Teleports[teleport].X = reader.ReadByte();

				reader.BaseStream.Position = Data.Address(0, 0xAD40);

				for (var teleport = 0; teleport < Map.Teleports.Length; teleport++)
					Map.Teleports[teleport].Y = reader.ReadByte();

				// Load Segments
				reader.BaseStream.Position = Data.Address(4, 0x8000 + (map * 2));

				var address = reader.ReadUInt16();

				var bank = 4 + (address >> 14);
				var offset = 0x8000 + (address & 0x3fff);

				reader.BaseStream.Position = Data.Address(bank, offset);

				var segments = new List<Map.Segment>();

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

					segments.Add(new Map.Segment { Tile = value, Count = count });
				}

				Map.Segments = segments.ToArray();
			}
		}
	}
}
