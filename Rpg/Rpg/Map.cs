using System;

namespace Rpg
{
	public static class Map
	{
		public static Zone[] Zones;
		public static int Width;
		public static int Height;

		public class Zone
		{
			public char Character;
			public int Top;
			public int Bottom;
			public int Left;
			public int Right;

			public string Description;
		}
	}
}