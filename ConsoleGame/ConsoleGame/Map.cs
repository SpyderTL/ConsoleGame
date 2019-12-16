using System;

namespace ConsoleGame
{
	internal static class Map
	{
		internal static int Width;
		internal static int Height;
		internal static int X;
		internal static int Y;
		internal static Zone[] Zones;

		internal static event Action PositionChanged;

		internal static void North()
		{
			if (Y == 0)
				Y = Height - 1;
			else
				Y--;

			PositionChanged?.Invoke();
		}

		internal static void South()
		{
			if (Y == Height - 1)
				Y = 0;
			else
				Y++;

			PositionChanged?.Invoke();
		}

		internal static void East()
		{
			if (X == Width - 1)
				X = 0;
			else
				X++;

			PositionChanged?.Invoke();
		}

		internal static void West()
		{
			if (X == 0)
				X = Width - 1;
			else
				X--;

			PositionChanged?.Invoke();
		}

		internal class Zone
		{
			internal char Character;
			internal int Top;
			internal int Bottom;
			internal int Left;
			internal int Right;
		}
	}
}