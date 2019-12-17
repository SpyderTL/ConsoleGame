using System;
using System.IO;

namespace RpgGame
{
	internal static class Data
	{
		internal static byte[] Rom = Properties.Resources.data;

		internal static int Address(int bank, int offset)
		{
			return (bank * 0x4000) + offset;
		}

		internal static BinaryReader Reader()
		{
			var stream = new MemoryStream(Rom);

			var reader = new BinaryReader(stream);

			return reader;
		}
	}
}
