using System;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	public static class World
	{
		public const int TileCount = 128;
		public const int TeleportCount = 32;
		public const int DomainCount = 64;
		public const int DomainFormationCount = 8;

		public static Tile[] Tiles = new Tile[TileCount];
		public static Teleport[] Teleports = new Teleport[TeleportCount];
		public static Row[] Rows;
		public static Domain[] Domains = new Domain[DomainCount];
			
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
