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
