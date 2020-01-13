using System;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	public static class Party
	{
		public static Character[] Characters;

		public class Character
		{
			public string Name;
			public int Type;
			public int Health;
			public int Power;
		}
	}
}
