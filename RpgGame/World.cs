﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	public static class World
	{
		public static Tile[] Tiles = new Tile[128];
		public static Teleport[] Teleports = new Teleport[32];
		public static Row[] Rows;
		public static Domain[] Domains = new Domain[64];
			
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
			public bool Blocked;
			public bool Dock;
			public bool Forest;
			public TileType Type;
			public bool Teleport;
			public bool Battle;
			public int Value;
		}

		public enum TileType
		{
			Normal,
			Chime,
			Caravan,
			Floater
		}

		public struct Teleport
		{
			public int Map;
			public int X;
			public int Y;
		}

		public struct Domain
		{
			public DomainFormation[] Formations;
		}

		public struct DomainFormation
		{
			public int Formation;
			public bool Alternate;
		}
	}
}
