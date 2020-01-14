using System;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	public static class Party
	{
		public static Character[] Characters = new Character[4];

		public class Character
		{
			public string Name;
			public CharacterType Type;
			public int Health;
			public int MaxHealth;
			public int Power;
			public int MaxPower;
		}

		public enum CharacterType
		{
			Fighter,
			BlackBelt
		}
	}
}
