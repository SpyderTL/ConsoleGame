using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame
{
	internal static class Fight
	{
		internal static Character[] Allies;
		internal static Character[] Enemies;

		internal class Character
		{
			internal string Name;
			internal int Health;
			internal int MaxHealth;
			internal int Power;
			internal int MaxPower;
			internal bool Fast;
			internal bool Slow;
			internal bool Poison;
			internal bool Sleep;
			internal bool Silence;
			internal bool Fire;
			internal bool Ice;
		}
	}
}
