using System;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	public static class Battle
	{
		public static event Action TurnStarted;
		public static event Action TurnComplete;
		public static event Action BattleStarted;
		public static event Action BattleComplete;

		public static Enemy[] Enemies;
		public static Activity[][] AllyOptions;
		public static Activity[][] EnemyOptions;
		public static int[] AllyActions;
		public static int[] EnemyActions;
		public static Event[] Events;
		public static BattleResults Result;

		public struct Enemy
		{
			public int Type;
			public int Health;
			public int Power;
		}

		public struct Activity
		{
			public ActivityType Type;
			public int Value;
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

		public struct Event
		{
			public EventType Type;
			public int Source;
			public SourceType SourceType;
			public int Target;
			public TargetType TargetType;
			public int Value;
		}

		public enum SourceType
		{
			Enemy,
			Ally,
		}

		public enum EventType
		{
			Attack,
			Special,
			Magic,
			Item,
			Run,
			Miss,
			Health,
			Power,
			Inflict,
			Cure,
			Resist,
			Weak
		}

		public enum BattleResults
		{
			Victory,
			Defeat,
			Escape
		}
	}
}
