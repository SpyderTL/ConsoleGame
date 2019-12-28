using System;
using System.Collections.Generic;
using System.Linq;
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
		public static event Action<int> TreasureFound;

		public static bool North()
		{
			var y = Y - 1;

			if (y == -1)
				y = 63;

			var segment = GetSegment(X, y);

			if (!Move(Rows[y][segment].Tile))
				return false;

			Y = y;
			PositionChanged?.Invoke();

			Treasure(Rows[Y][segment].Tile);

			Teleport(Rows[Y][segment].Tile);

			return true;
		}

		public static bool South()
		{
			var y = Y + 1;

			if (y == 64)
				y = 0;

			var segment = GetSegment(X, y);

			if (!Move(Rows[y][segment].Tile))
				return false;

			Y = y;
			PositionChanged?.Invoke();

			Treasure(Rows[Y][segment].Tile);

			Teleport(Rows[Y][segment].Tile);

			return true;
		}

		public static bool East()
		{
			var x = X + 1;

			if (x == 64)
				x = 0;

			var segment = GetSegment(x, Y);

			if (!Move(Rows[Y][segment].Tile))
				return false;

			X = x;
			PositionChanged?.Invoke();

			Treasure(Rows[Y][segment].Tile);

			Teleport(Rows[Y][segment].Tile);

			return true;
		}

		public static bool West()
		{
			var x = X - 1;

			if (x == -1)
				x = 63;

			var segment = GetSegment(x, Y);

			if (!Move(Rows[Y][segment].Tile))
				return false;

			X = x;
			PositionChanged?.Invoke();

			Treasure(Rows[Y][segment].Tile);

			Teleport(Rows[Y][segment].Tile);

			return true;
		}

		private static bool Move(int tile)
		{
			switch (Map.Tiles[tile].TileType)
			{
				case Map.TileType.Door:
					return true;

				case Map.TileType.Locked:
					return false;

				case Map.TileType.Treasure:
					return true;

				default:
					return !Map.Tiles[tile].Blocked;
			}
		}

		private static void Teleport(int tile)
		{
			switch (Map.Tiles[tile].TeleportType)
			{
				case Map.TeleportType.Normal:
					Floors.Push(new Floor
					{
						Map = Current,
						X = X,
						Y = Y
					});

					var teleport = Map.Tiles[tile].Value;

					DataMap.Load(Map.Teleports[teleport].Map);
					Current = Map.Teleports[teleport].Map;
					X = Map.Teleports[teleport].X;
					Y = Map.Teleports[teleport].Y;
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

				case Map.TeleportType.Exit:
					Floors.Clear();

					var exit = Map.Tiles[tile].Value;

					PartyWorld.X = Map.Exits[exit].X;
					PartyWorld.Y = Map.Exits[exit].Y;

					MapExited?.Invoke();
					break;
			}
		}

		private static void Treasure(int tile)
		{
			if (Map.Tiles[tile].TileType == Map.TileType.Treasure)
			{
				var treasure = Map.Tiles[tile].Value;

				if (!Map.Treasures[treasure].Opened)
				{
					Map.Treasures[treasure].Opened = true;

					TreasureFound?.Invoke(Map.Treasures[treasure].Item);
				}
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
