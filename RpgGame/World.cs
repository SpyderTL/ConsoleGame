using System;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	public static class World
	{
		public static Tile[] Tiles = new Tile[128];
		public static Row[] Rows;
			
		public struct Row
		{
			public Segment[] Segments;
		}

		public struct Segment
		{
			public int Tile;
			public int Count;
		}

		public struct Tile
		{
			public TileType TileType;
			public bool Blocked;
			public bool Battle;
			public TeleportType TeleportType;
			public int Value;
		}

		public enum TileType
		{
			Normal,
			Door,
			Locked,
			CloseRoom,
			Treasure,
			Battle,
			Damage,
			Crown,
			Cube,
			FourOrbs,
			UseRod,
			UseLute,
			EarthOrb,
			FireOrb,
			WaterOrb,
			AirOrb
		}

		public enum TeleportType
		{
			None,
			Warp,
			Normal,
			Exit
		}
	}
}
