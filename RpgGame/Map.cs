﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	public static class Map
	{
		public static Tile[] Tiles = new Tile[128];
		public static Segment[] Segments;

		public struct Segment
		{
			public int Tile;
			public int Count;
		}

		public struct Tile
		{
			public bool Blocked;
			public TileType TileType;
			public TeleportType TeleportType;
			public bool Battle;
			public byte Value;
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
