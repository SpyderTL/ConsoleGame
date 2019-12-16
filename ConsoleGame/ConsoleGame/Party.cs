using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame
{
	internal static class Party
	{
		internal static Character[] Characters = new Character[4];

		internal class Character
		{
			internal string Name;
			internal int Health;
			internal int MaxHealth;
			internal int Power;
			internal int MaxPower;
		}
	}
}
