using System;

namespace ConsoleGame
{
	internal static class Map
	{
		internal static Zone[] Zones;
		
		internal class Zone
		{
			internal char Character;
			internal int Top;
			internal int Bottom;
			internal int Left;
			internal int Right;

			public string Description;
		}
	}
}