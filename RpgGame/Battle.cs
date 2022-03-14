using System;
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
		public static Status[] AllyStatuses;
		public static Activity[][] EnemyOptions;
		public static Status[] EnemyStatuses;
		public static int[] AllyActions;
		public static int[] EnemyActions;
		public static int[] AllyHitMultipliers;
		public static int[] EnemyHitMultipliers;

		public static Event[] Events;

		public static BattleResults Result;

		public static EnemyType[] EnemyTypes = new EnemyType[128];
		public static LogicType[] LogicTypes = new LogicType[128];
		public static MagicType[] SpellTypes = new MagicType[64];
		public static MagicType[] PotionTypes = new MagicType[2];
		public static MagicType[] AbilityTypes = new MagicType[26];

		private static readonly Random Random = new Random();

		public static void Enable()
		{
			Result = BattleResults.None;

			AllyActions = Enumerable.Repeat(-1, Party.Characters.Length).ToArray();
			EnemyActions = Enumerable.Repeat(-1, Enemies.Length).ToArray();

			AllyOptions = Enumerable.Repeat(new Activity[0], Party.Characters.Length).ToArray();
			EnemyOptions = Enumerable.Repeat(new Activity[0], Enemies.Length).ToArray();

			AllyStatuses = Enumerable.Repeat(Status.None, Party.Characters.Length).ToArray();
			EnemyStatuses = Enumerable.Repeat(Status.None, Enemies.Length).ToArray();

			BattleStarting?.Invoke();

			UpdateOptions();

			TurnStarting?.Invoke();
		}

		private static void UpdateOptions()
		{
			for (var ally = 0; ally < Party.Characters.Length; ally++)
			{
				var options = new List<Activity>();

				options.AddRange(Enemies.Where((x, i) => !EnemyStatuses[i].HasFlag(Status.Dead)).Select((x, i) => new Activity { Type = ActivityType.Attack, TargetType = TargetType.Enemy, Target = i }));
				options.Add(new Activity { Type = ActivityType.Run });

				AllyOptions[ally] = options.ToArray();
				AllyActions[ally] = -1;
			}

			//for (var enemy = 0; enemy < Enemies.Length; enemy++)
			//{
			//	var options = new List<Activity>();

			//	options.AddRange(Party.Characters.Select((x, i) => new Activity { Type = ActivityType.Attack, TargetType = TargetType.Ally, Target = i }));
			//	// Should this be here?
			//	//options.Add(new Activity { Type = ActivityType.Run });

			//	var enemyType = EnemyTypes[Enemies[enemy].Type];

			//	if (enemyType.Logic != 0xFF)
			//	{
			//		var logicType = LogicTypes[enemyType.Logic];

			//		foreach (var spell in logicType.Spells)
			//		{
			//			if (spell != 0xFF)
			//			{
			//				var spellType = SpellTypes[spell];

			//				switch (spellType.Target)
			//				{
			//					case MagicTarget.Enemy:
			//						options.AddRange(Party.Characters.Select((x, i) => new Activity { Type = ActivityType.Spell, Value = spell, TargetType = TargetType.Ally, Target = i }));
			//						break;
			//				}
			//			}
			//		}

			//		foreach (var ability in logicType.Abilities)
			//		{
			//			if (ability != 0xFF)
			//			{
			//				var abilityType = AbilityTypes[ability];

			//				switch (abilityType.Target)
			//				{
			//					case MagicTarget.Enemy:
			//						options.AddRange(Party.Characters.Select((x, i) => new Activity { Type = ActivityType.Ability, Value = ability, TargetType = TargetType.Ally, Target = i }));
			//						break;
			//				}
			//			}
			//		}
			//	}

			//	EnemyOptions[enemy] = options.ToArray();

			//	EnemyActions[enemy] = -1;
			//}
		}

		public static void UpdateEvents()
		{
			var events = new List<Event>();

			// Randomize order
			var allyActions = Party.Characters.Select((x, i) => new
			{
				Action = AllyOptions[i][AllyActions[i]],
				Source = i,
				SourceType = SourceType.Ally,
				Party.Characters[i].Hits,
				Party.Characters[i].Accuracy,
				Party.Characters[i].Damage,
			});

			var enemyActions = Enemies.Select((x, i) => new
			{
				Action = EnemyAction(i),
				Source = i,
				SourceType = SourceType.Enemy,
				EnemyTypes[Enemies[i].Type].Hits,
				EnemyTypes[Enemies[i].Type].Accuracy,
				EnemyTypes[Enemies[i].Type].Damage,
			});

			var characterActions = allyActions.Concat(enemyActions).ToArray();

			for (var x = 0; x < 16; x++)
			{
				var a = Random.Next(0, characterActions.Length);
				var b = Random.Next(0, characterActions.Length);

				var temp = characterActions[a];
				characterActions[a] = characterActions[b];
				characterActions[b] = temp;
			}

			foreach (var action in characterActions)
			{
				switch (action.Action.Type)
				{
					case ActivityType.Attack:
						if (action.Action.TargetType == TargetType.Ally)
						{
							if (AllyStatuses[action.Action.Target].HasFlag(Status.Stone) ||
								AllyStatuses[action.Action.Target].HasFlag(Status.Dead))
								continue;
						}
						else if (EnemyStatuses[action.Action.Target].HasFlag(Status.Stone) ||
								EnemyStatuses[action.Action.Target].HasFlag(Status.Dead))
							continue;

						var hits = 0;
						var damage = 0;

						for (var hit = 0; hit < action.Hits; hit++)
						{
							var chance = 168;

							if (action.SourceType == SourceType.Ally)
							{
								if (AllyStatuses[action.Source].HasFlag(Status.Dark))
									chance -= 40;

								// TODO: Check weakness
							}
							else
							{
								if (EnemyStatuses[action.Source].HasFlag(Status.Dark))
									chance -= 40;
							}

							if (action.Action.TargetType == TargetType.Enemy)
							{
								if (EnemyStatuses[action.Action.Target].HasFlag(Status.Dark))
									chance += 40;
							}
							else
							{
								if (AllyStatuses[action.Action.Target].HasFlag(Status.Dark))
									chance += 40;
							}

							chance += action.Accuracy;

							if (chance > 255)
								chance = 255;

							if (action.Action.TargetType == TargetType.Enemy)
							{
								chance -= EnemyTypes[Enemies[action.Action.Target].Type].Evade;
							}
							else if (action.Action.TargetType == TargetType.Ally)
							{
								chance -= Party.Characters[action.Action.Target].Agility + 48;
							}

							if (chance < 0)
								chance = 0;

							var criticalChance = 0;

							// TODO: Calculate Critical Hit Chance

							var success = false;
							var critical = false;

							var value = Random.Next(201);

							if (value != 200 &&
								value <= chance)
							{
								success = true;

								if (value <= criticalChance)
									critical = true;
							}

							if (success)
							{
								hits++;

								damage += action.Damage;
								damage += Random.Next(action.Damage) + 1;
							}
						}

						if (hits != 0)
						{
							events.Add(new Event { Type = EventType.Hit, Source = action.Source, SourceType = action.SourceType, Target = action.Action.Target, TargetType = action.Action.TargetType, Value = hits });

							events.Add(new Event { Type = EventType.Health, Source = action.Source, SourceType = action.SourceType, Target = action.Action.Target, TargetType = action.Action.TargetType, Value = -damage });

							if (action.Action.TargetType == TargetType.Ally)
							{
								Party.Characters[action.Action.Target].Health -= Math.Min(damage, Party.Characters[action.Action.Target].Health);

								if (Party.Characters[action.Action.Target].Health == 0)
									AllyStatuses[action.Action.Target] |= Status.Dead;
							}
							else
							{
								Enemies[action.Action.Target].Health -= Math.Min(damage, Enemies[action.Action.Target].Health);

								if (Enemies[action.Action.Target].Health == 0)
									EnemyStatuses[action.Action.Target] |= Status.Dead;
							}
						}
						else
							events.Add(new Event { Type = EventType.Miss, Source = action.Source, SourceType = action.SourceType, Target = action.Action.Target, TargetType = action.Action.TargetType });
						break;

					case ActivityType.Ability:
						events.Add(new Event { Type = (EventType)action.Action.Type, Source = action.Source, SourceType = action.SourceType, Target = action.Action.Target, TargetType = action.Action.TargetType, Value = action.Action.Value });

						events.AddRange(Magic(AbilityTypes[action.Action.Value], action.Action, action.Source, action.SourceType, action.Action.Target, action.Action.TargetType, action.Accuracy, action.Hits, action.Damage));
						break;

					case ActivityType.Spell:
						events.Add(new Event { Type = (EventType)action.Action.Type, Source = action.Source, SourceType = action.SourceType, Target = action.Action.Target, TargetType = action.Action.TargetType, Value = action.Action.Value });

						events.AddRange(Magic(SpellTypes[action.Action.Value], action.Action, action.Source, action.SourceType, action.Action.Target, action.Action.TargetType, action.Accuracy, action.Hits, action.Damage));
						break;

					case ActivityType.Item:
						events.Add(new Event { Type = (EventType)action.Action.Type, Source = action.Source, SourceType = action.SourceType, Target = action.Action.Target, TargetType = action.Action.TargetType, Value = action.Action.Value });

						events.AddRange(Magic(PotionTypes[action.Action.Value], action.Action, action.Source, action.SourceType, action.Action.Target, action.Action.TargetType, action.Accuracy, action.Hits, action.Damage));
						break;

					case ActivityType.Run:
						if (Random.Next(2) == 0)
						{
							events.Add(new Event { Type = EventType.Escape, Source = action.Source, SourceType = action.SourceType, Target = action.Action.Target, TargetType = action.Action.TargetType });

							Result = BattleResults.Escape;
						}
						else
							events.Add(new Event { Type = EventType.Trapped, Source = action.Source, SourceType = action.SourceType, Target = action.Action.Target, TargetType = action.Action.TargetType });
						break;
				}

				if (AllyStatuses.All(x => x.HasFlag(Status.Stone) ||
					x.HasFlag(Status.Dead)) ||
					EnemyStatuses.All(x => x.HasFlag(Status.Stone) ||
						x.HasFlag(Status.Dead)))
					break;
			}

			Events = events.ToArray();
		}

		private static Activity EnemyAction(int enemy)
		{
			if (Enemies[enemy].Statuses.HasFlag(Status.Stun) ||
				Enemies[enemy].Statuses.HasFlag(Status.Stone) ||
				Enemies[enemy].Statuses.HasFlag(Status.Dead))
			{
				return new Activity
				{
					Type = ActivityType.None
				};
			}

			return new Activity
			{
				Type = ActivityType.None
			};
		}

		private static IEnumerable<Event> Magic(MagicType type, Activity action, int source, SourceType sourceType, int target, TargetType targetType, int accuracy, int hits, int damage)
		{
			switch (type.Effect)
			{
				case MagicEffect.Damage:
					damage = action.Value;
					damage += Random.Next(action.Value) + 1;

					switch (targetType)
					{
						case TargetType.Ally:
							Party.Characters[target].Health -= Math.Min(damage, Party.Characters[target].Health);

							if (Party.Characters[target].Health == 0)
								AllyStatuses[target] |= Status.Dead;

							yield return new Event { Type = EventType.Health, Source = source, SourceType = sourceType, Target = target, TargetType = targetType, Value = -damage };
							break;

						case TargetType.Enemy:
							Enemies[target].Health -= Math.Min(damage, Enemies[target].Health);

							if (Enemies[target].Health == 0)
								EnemyStatuses[target] |= Status.Dead;

							yield return new Event { Type = EventType.Health, Source = source, SourceType = sourceType, Target = target, TargetType = targetType, Value = -damage };
							break;

						case TargetType.Allies:
							for (var character = 0; character < Party.Characters.Length; character++)
							{
								Party.Characters[character].Health -= Math.Min(damage, Party.Characters[character].Health);

								if (Party.Characters[character].Health == 0)
									AllyStatuses[character] |= Status.Dead;

								yield return new Event { Type = EventType.Health, Source = source, SourceType = sourceType, Target = character, TargetType = targetType, Value = -damage };
							}
							break;

						case TargetType.Enemies:
							for (var enemy = 0; enemy < Enemies.Length; enemy++)
							{
								Enemies[enemy].Health -= Math.Min(damage, Enemies[enemy].Health);

								if (Enemies[enemy].Health == 0)
									EnemyStatuses[enemy] |= Status.Dead;

								yield return new Event { Type = EventType.Health, Source = source, SourceType = sourceType, Target = enemy, TargetType = targetType, Value = -damage };
							}
							break;
					}
					break;

				default:
					System.Diagnostics.Debugger.Break();
					yield break;
			}
		}

		public static void Update()
		{
			if (AllyActions.All(x => x != -1))
			{
				UpdateEvents();

				TurnComplete?.Invoke();

				if (AllyStatuses.All(x => x.HasFlag(Status.Stone) ||
					x.HasFlag(Status.Dead)))
				{
					Result = BattleResults.Defeat;
					BattleComplete?.Invoke();
				}
				else if (EnemyStatuses.All(x => x.HasFlag(Status.Stone) ||
					x.HasFlag(Status.Dead)))
				{
					Result = BattleResults.Victory;
					BattleComplete?.Invoke();
				}
				else
				{
					UpdateOptions();

					TurnStarting?.Invoke();
				}
			}

		}

		public static void Disable()
		{
		}

		public struct Enemy
		{
			public int Type;
			public int Health;
			public int MaxHealth;
			public int Power;
			public int MaxPower;
			public Status Statuses;
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
			Run,
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
			public int Accuracy;
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
