using System;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	public static class Battle
	{
		public static event Action TurnComplete;
		public static event Action BattleComplete;

		public static Enemy[] Enemies;
		public static Activity[] PartyActions;
		public static Activity[] EnemyActions;
		public static BattleResult Result;

		public struct Enemy
		{
			public string Name;
			public int Type;
			public int Health;
			public int Power;
		}

		public struct Activity
		{
			public ActivityType Type;
			public int Action;
			public TargetType TargetType;
			public int Target;
		}

		public enum ActivityType
		{
			Attack,
			Special,
			Magic,
			Item,
			Run
		}

		public enum TargetType
		{
			Enemy,
			Enemies,
			Ally,
			Allies
		}

		public enum BattleResult
		{
			Victory,
			Defeat,
			Escape
		}
	}
}
