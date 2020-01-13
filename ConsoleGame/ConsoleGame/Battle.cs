using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame
{
	internal static class Battle
	{
		internal static BattleMode Mode;
		internal static Character[] Allies;
		internal static Activity[][] AllyOptions;
		internal static int[] AllyActions;
		internal static Character[] Enemies;
		internal static Event[] TurnEvents;

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

		public enum SourceType
		{
			Enemy,
			Ally,
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

		internal enum BattleMode
		{
			BattleStarting,
			TurnStarting,
			TurnComplete,
			BattleComplete
		}
	}
}
