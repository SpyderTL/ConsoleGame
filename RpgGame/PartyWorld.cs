using System;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	public static class PartyWorld
	{
		public static int X;
		public static int Y;
		public static int Segment;

		public static MapSegment[][] Rows;

		public static event Action PositionChanged;
		public static event Action MapChanged;

		public static bool North()
		{
			var y = Y - 1;

			if (y == -1)
				y = 255;

			var segment = GetSegment(X, y);

			//if (World.Tiles[Rows[y][segment].Tile].Blocked)
			//	return false;

			Y = y;
			PositionChanged?.Invoke();

			Teleport(segment);

			return true;
		}

		public static bool South()
		{
			var y = Y + 1;

			if (y == 256)
				y = 0;

			var segment = GetSegment(X, y);

			//if (World.Tiles[Rows[y][segment].Tile].Blocked)
			//	return false;

			Y = y;
			PositionChanged?.Invoke();

			Teleport(segment);

			return true;
		}

		public static bool East()
		{
			var x = X + 1;

			if (x == 256)
				x = 0;

			var segment = GetSegment(x, Y);

			//if (World.Tiles[Rows[Y][segment].Tile].Blocked)
			//	return false;

			X = x;
			PositionChanged?.Invoke();

			Teleport(segment);

			return true;
		}

		public static bool West()
		{
			var x = X - 1;

			if (x == -1)
				x = 255;

			var segment = GetSegment(x, Y);

			//if (World.Tiles[Rows[Y][segment].Tile].Blocked)
			//	return false;

			X = x;
			PositionChanged?.Invoke();

			Teleport(segment);

			return true;
		}

		private static void Teleport(int segment)
		{
			if (World.Tiles[World.Rows[Y].Segments[segment].Tile].Teleport)
			{
				var teleport = World.Tiles[World.Rows[Y].Segments[segment].Tile].Value;

				DataMap.Load(World.Teleports[teleport].Map);
				PartyMap.Current = World.Teleports[teleport].Map;
				PartyMap.X = World.Teleports[teleport].X;
				PartyMap.Y = World.Teleports[teleport].Y;
				PartyMap.Refresh();

				MapChanged?.Invoke();
			}
		}

		public static void Refresh()
		{
			Rows = new MapSegment[World.Rows.Length][];

			for (var row = 0; row < Rows.Length; row++)
			{
				var x = 0;

				var segments = new List<MapSegment>();

				for (var segment = 0; segment < World.Rows[row].Segments.Length; segment++)
				{
					var count = World.Rows[row].Segments[segment].Count;

					segments.Add(new MapSegment
					{
						Left = x,
						Right = x + count - 1,
						Tile = World.Rows[row].Segments[segment].Tile
					});

					x += count;
				}

				Rows[row] = segments.ToArray();
			}
		}

		public static int GetSegment(int x, int y)
		{
			for (var segment = 0; segment < Rows[y].Length; segment++)
				if (Rows[y][segment].Left <= x &&
					Rows[y][segment].Right >= x)
					return segment;

			return 0;
		}

		public struct MapSegment
		{
			public int Left;
			public int Right;
			public int Tile;
		}
	}
}
