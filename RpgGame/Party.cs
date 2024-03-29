﻿using System;
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

			public int Level;
			public int Experience;

			public int Health;
			public int MaxHealth;
			public int Power;
			public int MaxPower;

			public int Strength;
			public int Agility;
			public int Intelligence;
			public int Vitality;
			public int Luck;

			public int Damage;
			public int Hits;
			public int Accuracy;
			public int Absorb;
			public int Evade;
		}

		public enum CharacterType
		{
			Fighter,
			Thief,
			BlackBelt,
			RedMage,
			WhiteMage,
			BlackMage
		}
	}
}
