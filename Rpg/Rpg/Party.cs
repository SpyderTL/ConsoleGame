using System;
using System.Collections.Generic;
using System.Text;

namespace Rpg
{
	public static class Party
	{
		public static int X;
		public static int Y;
		public static Character[] Characters = new Character[4];

		public class Character
		{
			public string Name;
			public int Health;
			public int MaxHealth;
			public int Power;
			public int MaxPower;
		}
	}
}
