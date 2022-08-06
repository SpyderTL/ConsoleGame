using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rpg
{
	public static class Battle
	{
		public static BattleMode Mode;
		public static Character[] Allies;
		public static Activity[][] Options;
		public static int[] Actions;
		public static Character[] Enemies;
		public static Event[] Events;

		public static string[] AbilityNames;
		public static string[] SpellNames;
		public static string[] ItemNames;

		public class Character
		{
			public string Name;
			public int Health;
			public int MaxHealth;
			public int Power;
			public int MaxPower;
			public bool Fast;
			public bool Slow;
			public bool Poison;
			public bool Sleep;
			public bool Silence;
			public bool Fire;
			public bool Ice;
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
			None,
			Attack,
			Ability,
			Spell,
			Item,
			Run
		}

		public enum TargetType
		{
			Enemy,
			Ally,
			Enemies,
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
			Status,
			Cure,
			Resist,
			Weak,
			Escape,
			Trapped,
			Evade
		}

		public enum BattleMode
		{
			BattleStarting,
			TurnStarting,
			TurnComplete,
			BattleComplete
		}
	}
}
