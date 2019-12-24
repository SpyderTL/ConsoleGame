using System;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	public static class World
	{
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
			public string Name;
			public int Walk;
			public int Fly;
			public int Sail;
			public bool Dock;
		}

		public static Tile[] Tiles = new Tile[]
		{
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },

			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = 1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },

			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },

			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },

			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },

			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },

			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },

			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = 1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" },
			new Tile { Walk = -1, Fly = 1, Sail = -1, Name = "" }
		};
	}
}
