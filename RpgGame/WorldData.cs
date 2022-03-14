using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame
{
	public static class WorldData
	{
		private const int TileTable = 0x8000;
		private const int TeleportXTable = 0xAC00;
		private const int TeleportYTable = 0xAC20;
		private const int TeleportMapTable = 0xAC40;
		private const int WorldSegmentBank = 0x01;
		private const int WorldSegmentTable = 0x8000;
		private const int DomainFormationBank = 0x0B;
		private const int DomainFormationTable = 0x8000;

		public static void Load()
		{
			using (var reader = Data.Reader())
			{
				// Load Tiles
				reader.BaseStream.Position = Data.Position(0, TileTable);

				for (var tile = 0; tile < 128; tile++)
				{
					var value = reader.ReadByte();
					var value2 = reader.ReadByte();

					World.Tiles[tile] = new World.Tile
					{
						Blocked = (value & 0x01) == 0x01,
						Forest = (value & 0x10) == 0x10,
						Dock = (value & 0x20) == 0x20,
						Type = (World.TileType)(value >> 6),
						Teleport = (value2 & 0x80) == 0x80,
						Battle = (value2 & 0x40) == 0x40,
						Value = value2 & 0x3f
					};
				}

				// Load Teleports
				reader.BaseStream.Position = Data.Position(0, TeleportMapTable);

				for (var teleport = 0; teleport < 32; teleport++)
					World.Teleports[teleport].Map = reader.ReadByte();

				reader.BaseStream.Position = Data.Position(0, TeleportXTable);

				for (var teleport = 0; teleport < 32; teleport++)
					World.Teleports[teleport].X = reader.ReadByte();

				reader.BaseStream.Position = Data.Position(0, TeleportYTable);

				for (var teleport = 0; teleport < 32; teleport++)
					World.Teleports[teleport].Y = reader.ReadByte();

				// Load Segments
				reader.BaseStream.Position = Data.Position(WorldSegmentBank, WorldSegmentTable);

				World.Rows = new World.Row[256];

				var rows = new int[256];

				for (var row = 0; row < 256; row++)
					rows[row] = reader.ReadUInt16();

				for (var row = 0; row < 256; row++)
				{
					reader.BaseStream.Position = Data.Position(1, rows[row]);

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

				// Load Domain Formations
				reader.BaseStream.Position = Data.Position(DomainFormationBank, DomainFormationTable);

				for (var domain = 0; domain < World.DomainCount; domain++)
				{
					World.Domains[domain].Formations = new World.DomainFormation[World.DomainFormationCount];

					for (var formation = 0; formation < World.DomainFormationCount; formation++)
					{
						var data = reader.ReadByte();

						World.Domains[domain].Formations[formation].Formation = data & 0x7f;
						World.Domains[domain].Formations[formation].Alternate = (data & 0x80) == 0x80;
					}
				}
			}
		}
	}
}
