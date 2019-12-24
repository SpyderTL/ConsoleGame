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
				reader.BaseStream.Position = Data.Address(1, 0x8000);

				World.Rows = new World.Row[256];

				var rows = new int[256];

				for (int row = 0; row < 256; row++)
					rows[row] = reader.ReadUInt16();

				for (int row = 0; row < 256; row++)
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
