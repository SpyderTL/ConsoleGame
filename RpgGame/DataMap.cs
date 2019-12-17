using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame
{
	public static class DataMap
	{
		public static void Load(int bank, int offset)
		{
			using (var reader = Data.Reader())
			{
				reader.BaseStream.Position = Data.Address(bank, offset);

				var rows = new int[256];

				for (int row = 0; row < 256; row++)
					rows[row] = reader.ReadUInt16();

				for (int row = 0; row < 256; row++)
				{
					reader.BaseStream.Position = Data.Address(bank, rows[row] - 0x8000);

					var segments = new List<Map.Segment>();

					while (true)
					{
						var value = reader.ReadByte();

						if (value == 0xff)
							break;

						var width = 1;

						if ((value & 0x80) == 0x80)
						{
							value &= 0x7f;

							width = reader.ReadByte();

							if (width == 0)
								width = 256;
						}

						segments.Add(new Map.Segment { Tile = value, Width = width });
					}

					Map.Rows[row].Segments = segments.ToArray();
				}
			}
		}
	}
}
