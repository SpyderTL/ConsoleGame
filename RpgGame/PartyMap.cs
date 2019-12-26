﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	public static class PartyMap
	{
		public static Stack<Floor> Floors = new Stack<Floor>();

		public static int Current;
		public static int X;
		public static int Y;

		public static MapSegment[][] Rows;

		public static event Action PositionChanged;
		public static event Action MapChanged;
		public static event Action MapExited;

		public static bool North()
		{
			if (Y == 0)
				return false;

			var segment = GetSegment(X, Y - 1);

			if (Map.Tiles[Rows[Y - 1][segment].Tile].Blocked)
				return false;

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

			if (Map.Tiles[Rows[Y + 1][segment].Tile].Blocked)
				return false;

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

			if (Map.Tiles[Rows[Y][segment].Tile].Blocked)
				return false;

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

			if (Map.Tiles[Rows[Y][segment].Tile].Blocked)
				return false;

			X--;
			PositionChanged?.Invoke();

			Teleport(segment);

			return true;
		}

		private static void Teleport(int segment)
		{
			switch (Map.Tiles[Rows[Y][segment].Tile].TeleportType)
			{
				case Map.TeleportType.Normal:
					Floors.Push(new Floor
					{
						Map = Current,
						X = X,
						Y = Y
					});

					var teleport = Map.Tiles[Rows[Y][segment].Tile].Value;

					DataMap.Load(World.Teleports[teleport].Map);
					Current = World.Teleports[teleport].Map;
					X = World.Teleports[teleport].X;
					Y = World.Teleports[teleport].Y;
					Refresh();

					MapChanged?.Invoke();
					break;

				case Map.TeleportType.Warp:
					if (Floors.Count == 0)
						MapExited?.Invoke();
					else
					{
						var floor = Floors.Pop();

						DataMap.Load(floor.Map);
						Current = floor.Map;
						X = floor.X;
						Y = floor.Y;
						Refresh();

						MapChanged?.Invoke();
					}
					break;
			}
		}

		public static void Refresh()
		{
			Rows = new MapSegment[64][];

			var y = 0;
			var left = 0;

			var segments = new List<MapSegment>();

			for (var segment = 0; segment < Map.Segments.Length; segment++)
			{
				var tile = Map.Segments[segment].Tile;
				var right = left + Map.Segments[segment].Count - 1;

				while (right > 63)
				{
					segments.Add(new MapSegment { Left = left, Right = 63, Tile = tile });

					Rows[y] = segments.ToArray();
					segments.Clear();

					right -= 64;
					left = 0;
					y++;
				}

				segments.Add(new MapSegment { Left = left, Right = right, Tile = tile });

				if (right == 63)
				{
					Rows[y] = segments.ToArray();
					segments.Clear();

					left = 0;
					right = 0;
					y++;
				}
				else
					left = right + 1;
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

		public struct Floor
		{
			public int Map;
			public int X;
			public int Y;
		}
	}
}
