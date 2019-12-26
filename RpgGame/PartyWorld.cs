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
			if (Y == 0)
				return false;

			var segment = GetSegment(X, Y - 1);

			//if (World.Tiles[World.Rows[Y - 1].Segments[segment].Tile].Blocked)
			//	return false;

			Y--;
			PositionChanged?.Invoke();

			Teleport(segment);

			return true;
		}

		public static bool South()
		{
			if (Y == Rows.Length - 1)
				return false;

			var segment = GetSegment(X, Y + 1);

			//if (World.Tiles[World.Rows[Y + 1].Segments[segment].Tile].Blocked)
			//	return false;

			Y++;
			PositionChanged?.Invoke();

			Teleport(segment);

			return true;
		}

		public static bool East()
		{
			if (X == 255)
				return false;

			var segment = GetSegment(X + 1, Y);

			//if (World.Tiles[World.Rows[Y].Segments[segment].Tile].Blocked)
			//	return false;

			X++;
			PositionChanged?.Invoke();

			Teleport(segment);

			return true;
		}

		public static bool West()
		{
			if (Y == 0)
				return false;

			var segment = GetSegment(X - 1, Y);

			//if (World.Tiles[World.Rows[Y].Segments[segment].Tile].Blocked)
			//	return false;

			X--;
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
