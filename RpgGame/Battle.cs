﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame
{
	public static class Battle
	{
		public static event Action TurnStarting;
		public static event Action TurnComplete;
		public static event Action BattleStarting;
		public static event Action BattleComplete;

		public static Enemy[] Enemies;
		public static Activity[][] AllyOptions;
		public static Activity[][] EnemyOptions;
		public static int[] AllyActions;
		public static int[] EnemyActions;
		public static Event[] Events;
		public static BattleResults Result;

		public static EnemyType[] EnemyTypes = new EnemyType[128];
		public static LogicType[] LogicTypes = new LogicType[128];
		public static MagicType[] SpellTypes = new MagicType[64];
		public static MagicType[] PotionTypes = new MagicType[2];
		public static MagicType[] AbilityTypes = new MagicType[26];

		public static System.Threading.Timer Timer;

		public static void Enable()
		{
			AllyActions = Enumerable.Repeat(-1, Party.Characters.Length).ToArray();
			EnemyActions = Enumerable.Repeat(-1, Enemies.Length).ToArray();

			AllyOptions = Enumerable.Repeat(new Activity[0], Party.Characters.Length).ToArray();
			EnemyOptions = Enumerable.Repeat(new Activity[0], Enemies.Length).ToArray();

			BattleStarting?.Invoke();

			UpdateOptions();

			TurnStarting?.Invoke();

			Timer = new System.Threading.Timer(Timer_Callback, null, 1000, 1000);
		}

		private static void UpdateOptions()
		{
			for (var ally = 0; ally < Party.Characters.Length; ally++)
			{
				var options = new List<Activity>();

				options.AddRange(Enumerable.Range(0, Enemies.Length).Select(x => new Activity { Type = ActivityType.Attack, TargetType = TargetType.Enemy, Target = x }));
				options.Add(new Activity { Type = ActivityType.Run });

				AllyOptions[ally] = options.ToArray();
				AllyActions[ally] = -1;
			}

			for (var enemy = 0; enemy < Enemies.Length; enemy++)
			{
				var options = new List<Activity>();

				options.AddRange(Enumerable.Range(0, Party.Characters.Length).Select(x => new Activity { Type = ActivityType.Attack, TargetType = TargetType.Ally, Target = x }));
				// Should this be here?
				//options.Add(new Activity { Type = ActivityType.Run });

				var enemyType = EnemyTypes[Enemies[enemy].Type];

				if (enemyType.Logic != 0xFF)
				{
					var logicType = LogicTypes[enemyType.Logic];

					foreach (var spell in logicType.Spells)
					{
						if (spell != 0xFF)
						{
							var spellType = SpellTypes[spell];

							switch (spellType.Target)
							{
								case MagicTarget.Enemy:
									options.AddRange(Enumerable.Range(0, Party.Characters.Length).Select(x => new Activity { Type = ActivityType.Spell, Value = spell, TargetType = TargetType.Ally, Target = x }));
									break;
							}
						}
					}

					foreach (var ability in logicType.Abilities)
					{
						if (ability != 0xFF)
						{
							var abilityType = AbilityTypes[ability];

							switch (abilityType.Target)
							{
								case MagicTarget.Enemy:
									options.AddRange(Enumerable.Range(0, Party.Characters.Length).Select(x => new Activity { Type = ActivityType.Ability, Value = ability, TargetType = TargetType.Ally, Target = x }));
									break;
							}
						}
					}
				}

				EnemyOptions[enemy] = options.ToArray();

				EnemyActions[enemy] = -1;
			}
		}

		public static void UpdateEvents()
		{
			var events = new List<Event>();

			var characters = Party.Characters.Select((x, i) => new
			{
				Action = AllyOptions[i][AllyActions[i]],
				Source = i,
				SourceType = SourceType.Ally,
				Hit = 0,
				Hits = 1,
				Damage = 10,
				Evade = AllyOptions[i][AllyActions[i]].TargetType == TargetType.Enemy ? EnemyTypes[Enemies[AllyOptions[i][AllyActions[i]].Target].Type].Evade : 0
			}).Concat(Enemies.Select((x, i) => new
			{
				Action = EnemyOptions[i][EnemyActions[i]],
				Source = i,
				SourceType = SourceType.Enemy,
				EnemyTypes[Enemies[i].Type].Hit,
				EnemyTypes[Enemies[i].Type].Hits,
				EnemyTypes[Enemies[i].Type].Damage,
				Evade = AllyOptions[i][AllyActions[i]].TargetType == TargetType.Enemy ? EnemyTypes[Enemies[AllyOptions[i][AllyActions[i]].Target].Type].Evade : 0
			})).ToArray();

			var random = new Random();

			for (var x = 0; x < 16; x++)
			{
				var a = random.Next(0, characters.Length);
				var b = random.Next(0, characters.Length);

				var temp = characters[a];
				characters[a] = characters[b];
				characters[b] = temp;
			}

			foreach (var character in characters)
			{
				switch (character.Action.Type)
				{
					case ActivityType.Attack:
						var hits = 0;
						var damage = 0;

						for (var hit = 0; hit < character.Hits; hit++)
						{
							var chance = 168 + character.Hit - character.Evade;

							if (random.Next(200) + 1 < chance)
							{
								hits++;

								damage += character.Damage;
								damage += random.Next(character.Damage) + 1;
							}
						}

						if(hits != 0)
						{
							events.Add(new Event { Type = EventType.Hit, Source = character.Source, SourceType = character.SourceType, Target = character.Action.Target, TargetType = character.Action.TargetType, Value = hits });

							events.Add(new Event { Type = EventType.Health, Source = character.Source, SourceType = character.SourceType, Target = character.Action.Target, TargetType = character.Action.TargetType, Value = -damage });
						}
						else
							events.Add(new Event { Type = EventType.Miss, Source = character.Source, SourceType = character.SourceType, Target = character.Action.Target, TargetType = character.Action.TargetType });
						break;

					case ActivityType.Ability:
						events.Add(new Event { Type = EventType.Ability, Source = character.Source, SourceType = character.SourceType, Target = character.Action.Target, TargetType = character.Action.TargetType, Value = character.Action.Value });
						break;

					case ActivityType.Spell:
						events.Add(new Event { Type = EventType.Spell, Source = character.Source, SourceType = character.SourceType, Target = character.Action.Target, TargetType = character.Action.TargetType, Value = character.Action.Value });
						break;

					case ActivityType.Item:
						events.Add(new Event { Type = EventType.Item, Source = character.Source, SourceType = character.SourceType, Target = character.Action.Target, TargetType = character.Action.TargetType, Value = character.Action.Value });
						break;

					case ActivityType.Run:
						if(random.Next(2) == 0)
							events.Add(new Event { Type = EventType.Escape, Source = character.Source, SourceType = character.SourceType, Target = character.Action.Target, TargetType = character.Action.TargetType });
						else
							events.Add(new Event { Type = EventType.Trapped, Source = character.Source, SourceType = character.SourceType, Target = character.Action.Target, TargetType = character.Action.TargetType });
						break;
				}
			}

			Events = events.ToArray();
		}

		private static void Timer_Callback(object state)
		{
			Timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

			if (AllyActions.All(x => x != -1) &&
				EnemyActions.All(x => x != -1))
			{
				UpdateEvents();

				TurnComplete?.Invoke();

				if (Party.Characters.All(x => x.Health == 0))
				{
					Disable();
					Result = BattleResults.Defeat;
					BattleComplete?.Invoke();
				}
				else if (Enemies.All(x => x.Health == 0))
				{
					Disable();
					Result = BattleResults.Victory;
					BattleComplete?.Invoke();
				}
				else
				{
					UpdateOptions();

					TurnStarting?.Invoke();

					Timer.Change(1000, 1000);
				}
			}
			else
				Timer.Change(1000, 1000);
		}

		public static void Disable()
		{
			Timer.Change(0, 0);
			Timer.Dispose();
			Timer = null;
		}

		public struct Enemy
		{
			public int Type;
			public int Health;
			public int MaxHealth;
			public int Power;
			public int MaxPower;
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

		public enum BattleResults
		{
			None,
			Victory,
			Defeat,
			Escape
		}

		public struct EnemyType
		{
			public string Name;
			public int Experience;
			public int Gold;
			public int Health;
			public int Morale;
			public int Logic;
			public int Evade;
			public int Absorb;
			public int Hits;
			public int Hit;
			public int Damage;
			public int Critical;
			public int Reserved;
			public int Attack;
			public EnemyCategories Categories;
			public int MagicDefense;
			public Elements Weak;
			public Elements Resist;
		}

		[Flags]
		public enum EnemyCategories
		{
			None = 0,
			Normal = 1,
			Dragon = 2,
			Giant = 4,
			Undead = 8,
			Were = 16,
			Water = 32,
			Mage = 64,
			Regen = 128
		}

		[Flags]
		public enum Elements
		{
			None = 0,
			Status = 1,
			Poison = 2,
			Time = 4,
			Death = 8,
			Fire = 16,
			Ice = 32,
			Lightning = 64,
			Earth = 128
		}

		public struct LogicType
		{
			public int Spell;
			public int Ability;
			public int[] Spells;
			public int[] Abilities;
		}

		public struct MagicType
		{
			public int Hit;
			public int Value;
			public Elements Elements;
			public MagicTarget Target;
			public MagicEffect Effect;
			public Elements EffectElements;
			public Status EffectStatus;
			public int Graphic;
			public int Palette;
			public int Reserved;
		}

		[Flags]
		public enum MagicTarget
		{
			None = 0,
			Enemies = 1,
			Enemy = 2,
			Self = 4,
			Allies = 8,
			Ally = 16
		}

		public enum MagicEffect
		{
			None,
			Damage,
			Holy,
			Status,
			Slow,
			Fear,
			Health,
			Health2,
			Cure,
			Absorb,
			Resist,
			Attack,
			Fast,
			Attack2,
			Stun,
			CureAll,
			Evade,
			Weak,
			Status2
		}

		[Flags]
		public enum Status
		{
			None = 0,
			Dead = 1,
			Stone = 2,
			Poison = 4,
			Dark = 8,
			Stun = 16,
			Sleep = 32,
			Mute = 64,
			Confused = 128
		}
	}
}
