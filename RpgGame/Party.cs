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
			string Name;
			CharacterType Type;
			int Health;
			int MaxHealth;
			int Power;
			int MaxPower;
			int Fast;
			int Slow;
			int Poison;
		}

		public enum CharacterType
		{
			Paladin
		}
	}
}
