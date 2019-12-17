using System;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	public static class Map
	{
		public static Row[] Rows;

		public struct Row
		{
			public Segment[] Segments;
		}

		public struct Segment
		{
			public int Tile;
			public int Width;
		}
	}
}
