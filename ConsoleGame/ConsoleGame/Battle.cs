using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame
{
	internal static class Battle
	{
		internal static BattleMode Mode;
		internal static Character[] Allies;
		internal static Activity[][] Options;
		internal static int[] Actions;
		internal static Character[] Enemies;
		internal static Event[] Events;

		internal static string[] AbilityNames;
		internal static string[] SpellNames;
		internal static string[] ItemNames;

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
			Ability,
			Spell,
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
			Hit,
			Ability,
			Spell,
			Item,
			Run,
			Miss,
			Health,
			Power,
			Inflict,
			Cure,
			Resist,
			Weak,
			Escape,
			Trapped
		}

		internal enum BattleMode
		{
			BattleStarting,
			TurnStarting,
			TurnComplete,
			BattleComplete
		}

		internal static void Start()
		{
			throw new NotImplementedException();
		}

		internal static void End()
		{
			throw new NotImplementedException();
		}
	}
}
