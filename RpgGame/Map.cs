using System;
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
			public string Name;
			public int Walk;
			public int Fly;
			public int Sail;
			public bool Dock;
			public int PortalMap;
			public int PortalX;
			public int PortalY;
		}
	}
}
