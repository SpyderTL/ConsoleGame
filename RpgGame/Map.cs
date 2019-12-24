using System;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	public static class Map
	{
		public static Segment[] Segments;

		public struct Segment
		{
			public int Tile;
			public int Repeat;
		}

		public struct Tile
		{
			public string Name;
			public int Walk;
			public int Fly;
			public int Sail;
			public bool Dock;
		}

		public static Tile[] Tiles = new Tile[128];
	}
}
