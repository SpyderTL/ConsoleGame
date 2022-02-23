using System;
using System.IO;

namespace RpgGame
{
	internal static class Data
	{
		internal static byte[] Rom = Properties.Resources.ROM;

		internal static int Position(int bank, int address)
		{
			return (bank * 0x4000) + address - 0x8000;
		}

		internal static BinaryReader Reader()
		{
			var stream = new MemoryStream(Rom);

			var reader = new BinaryReader(stream);

			return reader;
		}
	}
}
