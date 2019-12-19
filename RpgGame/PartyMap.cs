using System;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	public static class PartyMap
	{
		public static int X;
		public static int Y;
		public static int Segment;

		public static MapSegment[][] Rows;

		public static event Action PositionChanged;

		public static bool North()
		{
			if (Y == 0)
				return false;

			var segment = GetSegment(X, Y - 1);

			if (Map.Tiles[Map.Rows[Y].Segments[segment].Tile].Walk == -1)
				return false;


		}

		public static bool South()
		{
		}

		public static bool East()
		{
		}

		public static bool West()
		{
		}

		public static void Update()
		{
			Rows = new MapSegment[Map.Rows.Length][];

			var x = 0;

			for (var row = 0; row < Rows.Length; row++)
			{
				var segments = new List<MapSegment>();

				for (var segment = 0; segment < Map.Rows[row].Segments.Length; segment++)
				{
					var width = Map.Rows[row].Segments[segment].Width;

					segments.Add(new MapSegment
					{
						Left = x,
						Right = x + width - 1
					});

					x += width;
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
		}
	}
}
