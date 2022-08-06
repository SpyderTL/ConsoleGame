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
		public static int[] EnemySpellPositions;
		public static int[] EnemyAbilityPositions;
		public static int[] AllyEvades;

		public static Event[] Events;

		public static BattleResults Result;

		public static EnemyType[] EnemyTypes = new EnemyType[128];
		public static LogicType[] LogicTypes = new LogicType[128];
		public static MagicType[] SpellTypes = new MagicType[64];
		public static MagicType[] PotionTypes = new MagicType[2];
		public static MagicType[] AbilityTypes = new MagicType[26];
		public static FormationType[][] Formations = new FormationType[128][];
		private static readonly Random Random = new Random();

		public static void Enable()
		{
			Result = BattleResults.None;

			AllyActions = Enumerable.Repeat(-1, Party.Characters.Length).ToArray();
			EnemyActions = Enumerable.Repeat(-1, Enemies.Length).ToArray();

			AllyOptions = Enumerable.Repeat(new Activity[0], Party.Characters.Length).ToArray();
			EnemyOptions = Enumerable.Repeat(new Activity[0], Enemies.Length).ToArray();

			AllyStatuses = Party.Characters.Select(x => x.Health == 0 ? Status.Dead : Status.None).ToArray();
			EnemyStatuses = Enumerable.Repeat(Status.None, Enemies.Length).ToArray();

			AllyHitMultipliers = Enumerable.Repeat(1, Party.Characters.Length).ToArray();
			EnemyHitMultipliers = Enumerable.Repeat(1, Enemies.Length).ToArray();

			AllyEvades = Party.Characters.Select(x => x.Evade).ToArray();

			EnemySpellPositions = new int[Enemies.Length];
			EnemyAbilityPositions = new int[Enemies.Length];

			BattleStarting?.Invoke();

			UpdateOptions();

			TurnStarting?.Invoke();
		}

		private static void UpdateOptions()
		{
			for (var ally = 0; ally < Party.Characters.Length; ally++)
			{
				var options = new List<Activity>();

				if (!AllyStatuses[ally].HasFlag(Status.Stone) &&
					!AllyStatuses[ally].HasFlag(Status.Dead) &&
					!AllyStatuses[ally].HasFlag(Status.Sleep) &&
					!AllyStatuses[ally].HasFlag(Status.Stun))
				{
					for (var enemy = 0; enemy < Enemies.Length; enemy++)
					{
						if (!EnemyStatuses[enemy].HasFlag(Status.Dead))
							options.Add(new Activity { Type = ActivityType.Attack, TargetType = TargetType.Enemy, Target = enemy });
					}

					options.Add(new Activity { Type = ActivityType.Run });
				}

				AllyOptions[ally] = options.ToArray();
				AllyActions[ally] = -1;
			}
		}

		public static void UpdateEvents()
		{
			var events = new List<Event>();

			// Randomize order
			var allies = Enumerable.Range(0, Party.Characters.Length);

			var enemies = Enemies.Select((x, i) => i | 0x80);

			var characters = allies.Concat(enemies).ToArray();

			for (var x = 0; x < 16; x++)
			{
				var a = Random.Next(0, characters.Length);
				var b = Random.Next(0, characters.Length);

				var temp = characters[a];
				characters[a] = characters[b];
				characters[b] = temp;
			}

			foreach (var character in characters)
			{
				if ((character & 0x80) == 0)
					events.AddRange(AllyEvents(character));
				else
					events.AddRange(EnemyEvents(character & 0x7f));

				if (AllyStatuses.All(x => x.HasFlag(Status.Stone) ||
					x.HasFlag(Status.Dead)) ||
					EnemyStatuses.All(x => x.HasFlag(Status.Stone) ||
						x.HasFlag(Status.Dead)))
					break;
			}

			//foreach (var action in characterActions)
			//{
			//	if (action.SourceType == SourceType.Ally)
			//	{
			//		if (AllyStatuses[action.Source].HasFlag(Status.Stone) ||
			//			AllyStatuses[action.Source].HasFlag(Status.Dead))
			//			continue;

			//		if (AllyStatuses[action.Source].HasFlag(Status.Sleep))
			//		{
			//			AllyStatuses[action.Source] &= ~Status.Sleep;
			//			continue;
			//		}
			//	}
			//	else
			//	{
			//		if (EnemyStatuses[action.Source].HasFlag(Status.Stone) ||
			//			EnemyStatuses[action.Source].HasFlag(Status.Dead))
			//			continue;

			//		if (EnemyStatuses[action.Source].HasFlag(Status.Sleep))
			//		{
			//			EnemyStatuses[action.Source] &= ~Status.Sleep;
			//			continue;
			//		}
			//	}

			//	switch (action.Action.Value.Type)
			//	{
			//		case ActivityType.Attack:
			//			if (action.Action.Value.TargetType == TargetType.Ally)
			//			{
			//				if (AllyStatuses[action.Action.Target].HasFlag(Status.Stone) ||
			//					AllyStatuses[action.Action.Target].HasFlag(Status.Dead))
			//					continue;
			//			}
			//			else if (EnemyStatuses[action.Action.Target].HasFlag(Status.Stone) ||
			//					EnemyStatuses[action.Action.Target].HasFlag(Status.Dead))
			//				continue;

			//			var hits = 0;
			//			var damage = 0;

			//			for (var hit = 0; hit < action.Hits; hit++)
			//			{
			//				var chance = 168;

			//				if (action.SourceType == SourceType.Ally)
			//				{
			//					if (AllyStatuses[action.Source].HasFlag(Status.Dark))
			//						chance -= 40;

			//					// TODO: Check weakness
			//				}
			//				else
			//				{
			//					if (EnemyStatuses[action.Source].HasFlag(Status.Dark))
			//						chance -= 40;
			//				}

			//				if (action.Action.TargetType == TargetType.Enemy)
			//				{
			//					if (EnemyStatuses[action.Action.Target].HasFlag(Status.Dark))
			//						chance += 40;
			//				}
			//				else
			//				{
			//					if (AllyStatuses[action.Action.Target].HasFlag(Status.Dark))
			//						chance += 40;
			//				}

			//				chance += action.Accuracy;

			//				if (chance > 255)
			//					chance = 255;

			//				if (action.Action.TargetType == TargetType.Enemy)
			//				{
			//					chance -= EnemyTypes[Enemies[action.Action.Target].Type].Evade;
			//				}
			//				else if (action.Action.TargetType == TargetType.Ally)
			//				{
			//					chance -= Party.Characters[action.Action.Target].Agility + 48;
			//				}

			//				if (chance < 0)
			//					chance = 0;

			//				var criticalChance = 0;

			//				// TODO: Calculate Critical Hit Chance

			//				var success = false;
			//				var critical = false;

			//				var value = Random.Next(201);

			//				if (value != 200 &&
			//					value <= chance)
			//				{
			//					success = true;

			//					if (value <= criticalChance)
			//						critical = true;
			//				}

			//				if (success)
			//				{
			//					hits++;

			//					damage += action.Damage;
			//					damage += Random.Next(action.Damage) + 1;
			//				}
			//			}

			//			if (hits != 0)
			//			{
			//				events.Add(new Event { Type = EventType.Hit, Source = action.Source, SourceType = action.SourceType, Target = action.Action.Target, TargetType = action.Action.TargetType, Value = hits });

			//				events.Add(new Event { Type = EventType.Health, Source = action.Source, SourceType = action.SourceType, Target = action.Action.Target, TargetType = action.Action.TargetType, Value = -damage });

			//				if (action.Action.TargetType == TargetType.Ally)
			//				{
			//					Party.Characters[action.Action.Target].Health -= Math.Min(damage, Party.Characters[action.Action.Target].Health);

			//					if (Party.Characters[action.Action.Target].Health == 0)
			//						AllyStatuses[action.Action.Target] |= Status.Dead;
			//				}
			//				else
			//				{
			//					Enemies[action.Action.Target].Health -= Math.Min(damage, Enemies[action.Action.Target].Health);

			//					if (Enemies[action.Action.Target].Health == 0)
			//						EnemyStatuses[action.Action.Target] |= Status.Dead;
			//				}
			//			}
			//			else
			//				events.Add(new Event { Type = EventType.Miss, Source = action.Source, SourceType = action.SourceType, Target = action.Action.Target, TargetType = action.Action.TargetType });
			//			break;

			//		case ActivityType.Ability:
			//			events.Add(new Event { Type = (EventType)action.Action.Type, Source = action.Source, SourceType = action.SourceType, Target = action.Action.Target, TargetType = action.Action.TargetType, Value = action.Action.Value });

			//			events.AddRange(Magic(AbilityTypes[action.Action.Value], action.Action, action.Source, action.SourceType, action.Action.Target, action.Action.TargetType, action.Accuracy, action.Hits, action.Damage));
			//			break;

			//		case ActivityType.Spell:
			//			events.Add(new Event { Type = (EventType)action.Action.Type, Source = action.Source, SourceType = action.SourceType, Target = action.Action.Target, TargetType = action.Action.TargetType, Value = action.Action.Value });

			//			events.AddRange(Magic(SpellTypes[action.Action.Value], action.Action, action.Source, action.SourceType, action.Action.Target, action.Action.TargetType, action.Accuracy, action.Hits, action.Damage));
			//			break;

			//		case ActivityType.Item:
			//			events.Add(new Event { Type = (EventType)action.Action.Type, Source = action.Source, SourceType = action.SourceType, Target = action.Action.Target, TargetType = action.Action.TargetType, Value = action.Action.Value });

			//			events.AddRange(Magic(PotionTypes[action.Action.Value], action.Action, action.Source, action.SourceType, action.Action.Target, action.Action.TargetType, action.Accuracy, action.Hits, action.Damage));
			//			break;

			//		case ActivityType.Run:
			//			if (Random.Next(2) == 0)
			//			{
			//				events.Add(new Event { Type = EventType.Escape, Source = action.Source, SourceType = action.SourceType, Target = action.Action.Target, TargetType = action.Action.TargetType });

			//				Result = BattleResults.Escape;
			//			}
			//			else
			//				events.Add(new Event { Type = EventType.Trapped, Source = action.Source, SourceType = action.SourceType, Target = action.Action.Target, TargetType = action.Action.TargetType });
			//			break;
			//	}

			//	if (AllyStatuses.All(x => x.HasFlag(Status.Stone) ||
			//		x.HasFlag(Status.Dead)) ||
			//		EnemyStatuses.All(x => x.HasFlag(Status.Stone) ||
			//			x.HasFlag(Status.Dead)))
			//		break;
			//}

			Events = events.ToArray();
		}

		private static IEnumerable<Event> AllyEvents(int ally)
		{
			if (AllyStatuses[ally].HasFlag(Status.Stone) ||
				AllyStatuses[ally].HasFlag(Status.Dead) ||
				AllyStatuses[ally].HasFlag(Status.Stun))
				yield break;

			if (AllyStatuses[ally].HasFlag(Status.Sleep))
			{
				AllyStatuses[ally] &= ~Status.Sleep;
				yield return new Event { Type = EventType.Cure, Target = ally, TargetType = TargetType.Ally, Value = (int)Status.Sleep };
			}
			else
			{
				var action = AllyOptions[ally][AllyActions[ally]];

				switch (action.Type)
				{
					case ActivityType.Attack:

						if (action.TargetType == TargetType.Enemy)
						{
							foreach (var e in AttackEvents(SourceType.Ally, ally, Party.Characters[ally].Hits, Party.Characters[ally].Accuracy, Party.Characters[ally].Damage, AllyStatuses[ally], action.TargetType, action.Target, EnemyTypes[Enemies[action.Target].Type].Evade, EnemyTypes[Enemies[action.Target].Type].Absorb, EnemyStatuses[action.Target]))
								yield return e;
						}
						else
						{
							foreach (var e in AttackEvents(SourceType.Ally, ally, Party.Characters[ally].Hits, Party.Characters[ally].Accuracy, Party.Characters[ally].Damage, AllyStatuses[ally], action.TargetType, action.Target, Party.Characters[action.Target].Evade, Party.Characters[action.Target].Absorb, AllyStatuses[action.Target]))
								yield return e;
						}
						break;
				}
			}
		}

		private static IEnumerable<Event> AttackEvents(SourceType sourceType, int source, int sourceHits, int sourceAccuracy, int sourceDamage, Status sourceStatus, TargetType targetType, int target, int targetEvade, int targetAbsorb, Status targetStatus)
		{
			if (targetStatus.HasFlag(Status.Stone) ||
				targetStatus.HasFlag(Status.Dead))
				yield break;

			var hits = 0;
			var criticalHits = 0;
			var damage = 0;

			for (var hit = 0; hit < sourceHits; hit++)
			{
				var chance = 168;

				if (sourceStatus.HasFlag(Status.Dark))
					chance -= 40;

				if (targetStatus.HasFlag(Status.Dark))
					chance += 40;

				chance += sourceAccuracy;

				if (chance > 255)
					chance = 255;

				chance -= targetEvade;      // TODO: Allies get evade + 48

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
					var temp = sourceDamage;
					var temp2 = Random.Next(sourceDamage);

					if (critical)
					{
						criticalHits++;
						// Add Critical Damage

						temp2 += temp2;
					}

					damage = temp + temp2;

					if (damage > 255)
						damage = 255;

					damage -= targetAbsorb;

					if (damage < 1)
						damage = 1;
				}
			}

			if (hits != 0)
			{
				yield return new Event { Type = EventType.Hit, Source = source, SourceType = sourceType, Target = target, TargetType = targetType, Value = hits };

				// Notify Critical Hit

				yield return new Event { Type = EventType.Health, Source = source, SourceType = sourceType, Target = target, TargetType = targetType, Value = -damage };

				if (targetType == TargetType.Ally)
				{
					Party.Characters[target].Health -= Math.Min(damage, Party.Characters[target].Health);

					if (Party.Characters[target].Health == 0)
						AllyStatuses[target] |= Status.Dead;
				}
				else
				{
					Enemies[target].Health -= Math.Min(damage, Enemies[target].Health);

					if (Enemies[target].Health == 0)
						EnemyStatuses[target] |= Status.Dead;
				}
			}
			else
				yield return new Event { Type = EventType.Miss, Source = source, SourceType = sourceType, Target = target, TargetType = targetType };
		}

		private static IEnumerable<Event> EnemyEvents(int enemy)
		{
			var activityType = BattleLogic.EnemyActivityType(
				EnemyTypes[Enemies[enemy].Type].Logic,
				EnemyStatuses[enemy],
				Random.Next(128),
				Random.Next(128));

			switch (activityType)
			{
				case ActivityType.Attack:
					var target = BattleLogic.RandomAlly(Random.Next(256));

					while (AllyStatuses[target].HasFlag(Status.Stone) ||
						AllyStatuses[target].HasFlag(Status.Dead))
						target = BattleLogic.RandomAlly(Random.Next(256));

					foreach (var e in AttackEvents(SourceType.Enemy, enemy, EnemyTypes[Enemies[enemy].Type].Hits, EnemyTypes[Enemies[enemy].Type].Accuracy, EnemyTypes[Enemies[enemy].Type].Damage, EnemyStatuses[enemy], TargetType.Ally, target, Party.Characters[target].Evade, Party.Characters[target].Absorb, AllyStatuses[target]))
						yield return e;
					break;

				case ActivityType.Spell:
					if (LogicTypes[EnemyTypes[Enemies[enemy].Type].Logic].Spells[EnemySpellPositions[enemy]] == 0xFF)
						EnemySpellPositions[enemy] = 0;

					var spell = LogicTypes[EnemyTypes[Enemies[enemy].Type].Logic].Spells[EnemySpellPositions[enemy]];

					var spellEvents = SpellEvents(spell, enemy, SourceType.Enemy);

					EnemySpellPositions[enemy]++;

					if (EnemySpellPositions[enemy] == LogicTypes[EnemyTypes[Enemies[enemy].Type].Logic].Spells.Length)
						EnemySpellPositions[enemy] = 0;

					foreach (var e in spellEvents)
						yield return e;

					break;

				case ActivityType.Ability:
					if (LogicTypes[EnemyTypes[Enemies[enemy].Type].Logic].Abilities[EnemyAbilityPositions[enemy]] == 0xFF)
						EnemyAbilityPositions[enemy] = 0;

					var ability = LogicTypes[EnemyTypes[Enemies[enemy].Type].Logic].Abilities[EnemyAbilityPositions[enemy]];

					var abilityEvents = AbilityEvents(ability, enemy, SourceType.Enemy);

					EnemyAbilityPositions[enemy]++;

					if (EnemyAbilityPositions[enemy] == LogicTypes[EnemyTypes[Enemies[enemy].Type].Logic].Abilities.Length)
						EnemyAbilityPositions[enemy] = 0;

					foreach (var e in abilityEvents)
						yield return e;

					break;
			}
		}

		private static IEnumerable<Event> SpellEvents(int spell, int source, SourceType sourceType)
		{
			var effect = SpellTypes[spell].Effect;
			var value = SpellTypes[spell].Value;

			switch (effect)
			{
				case MagicEffect.Damage:
					return DamageSpellEvents(spell, value, source, sourceType);
				case MagicEffect.Evade:
					return EvadeSpellEvents(spell, value, source, sourceType);
				case MagicEffect.Fast:
					return FastSpellEvents(spell, value, source, sourceType);
				case MagicEffect.Slow:
					return SlowSpellEvents(spell, value, source, sourceType);
				case MagicEffect.Status:
					return StatusSpellEvents(spell, value, source, sourceType);
				case MagicEffect.ForceStatus:
					return ForceStatusSpellEvents(spell, value, source, sourceType);
				default:
					return Enumerable.Empty<Event>();
			}
		}

		private static IEnumerable<Event> AbilityEvents(int ability, int source, SourceType sourceType)
		{
			var effect = AbilityTypes[ability].Effect;
			var value = AbilityTypes[ability].Value;

			switch (effect)
			{
				case MagicEffect.Damage:
					return DamageAbilityEvents(ability, value, source, sourceType);
				case MagicEffect.Status:
					return StatusAbilityEvents(ability, value, source, sourceType);
				default:
					return Enumerable.Empty<Event>();
			}
		}

		private static IEnumerable<Event> DamageSpellEvents(int spell, int value, int source, SourceType sourceType)
		{
			var targetType = SpellTypes[spell].Target;

			switch (targetType)
			{
				case MagicTarget.Enemy:
					return EnemyDamageSpellEvents(spell, value, source, sourceType);
				case MagicTarget.Enemies:
					return EnemiesDamageSpellEvents(spell, value, source, sourceType);
				default:
					return Enumerable.Empty<Event>();
			}
		}

		private static IEnumerable<Event> DamageAbilityEvents(int ability, int value, int source, SourceType sourceType)
		{
			var targetType = AbilityTypes[ability].Target;

			switch (targetType)
			{
				case MagicTarget.Enemy:
					return EnemyDamageAbilityEvents(ability, value, source, sourceType);
				case MagicTarget.Enemies:
					return EnemiesDamageAbilityEvents(ability, value, source, sourceType);
				default:
					return Enumerable.Empty<Event>();
			}
		}

		private static IEnumerable<Event> EvadeSpellEvents(int spell, int value, int source, SourceType sourceType)
		{
			var targetType = SpellTypes[spell].Target;

			switch (targetType)
			{
				case MagicTarget.Self:
					return SelfEvadeSpellEvents(spell, value, source, sourceType, source, (TargetType)sourceType);
				default:
					return Enumerable.Empty<Event>();
			}
		}

		private static IEnumerable<Event> FastSpellEvents(int spell, int value, int source, SourceType sourceType)
		{
			var targetType = SpellTypes[spell].Target;

			switch (targetType)
			{
				case MagicTarget.Self:
					return SelfFastSpellEvents(spell, value, source, sourceType, source, (TargetType)sourceType);
				case MagicTarget.Ally:
					return AllyFastSpellEvents(spell, value, source, sourceType);
				default:
					return Enumerable.Empty<Event>();
			}
		}

		private static IEnumerable<Event> SlowSpellEvents(int spell, int value, int source, SourceType sourceType)
		{
			var targetType = SpellTypes[spell].Target;

			switch (targetType)
			{
				case MagicTarget.Enemy:
					return EnemySlowSpellEvents(spell, value, source, sourceType);
				case MagicTarget.Enemies:
					return EnemiesSlowSpellEvents(spell, value, source, sourceType);
				default:
					return Enumerable.Empty<Event>();
			}
		}

		private static IEnumerable<Event> EnemySlowSpellEvents(int spell, int value, int source, SourceType sourceType)
		{
			if (sourceType == SourceType.Enemy)
			{
				var ally = BattleLogic.RandomAlly(Random.Next(256));

				while(AllyStatuses[ally].HasFlag(Status.Stone) ||
					AllyStatuses[ally].HasFlag(Status.Dead))
					ally = BattleLogic.RandomAlly(Random.Next(256));

				yield return new Event { Type = EventType.Spell, Value = spell, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };

				if (AllyHitMultipliers[ally] > 0)
				{
					AllyHitMultipliers[ally]--;
					//yield return new Event { Type = EventType., Value = value, Source = source, SourceType = sourceType, Target = target, TargetType = targetType };
				}
			}
			else
			{
				yield break;
			}
		}

		private static IEnumerable<Event> EnemiesSlowSpellEvents(int spell, int value, int source, SourceType sourceType)
		{
			if (sourceType == SourceType.Enemy)
			{
				for (var ally = 0; ally < Party.Characters.Length; ally++)
				{
					if (AllyStatuses[ally].HasFlag(Status.Stone) ||
						AllyStatuses[ally].HasFlag(Status.Dead))
						continue;

					yield return new Event { Type = EventType.Spell, Value = spell, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };

					if (AllyHitMultipliers[ally] > 0)
					{
						AllyHitMultipliers[ally]--;
						//yield return new Event { Type = EventType., Value = value, Source = source, SourceType = sourceType, Target = target, TargetType = targetType };
					}
				}
			}
			else
			{
				yield break;
			}
		}

		private static IEnumerable<Event> StatusSpellEvents(int spell, int value, int source, SourceType sourceType)
		{
			var targetType = SpellTypes[spell].Target;
			var status = SpellTypes[spell].EffectStatus;

			switch (targetType)
			{
				case MagicTarget.Self:
					return SelfStatusSpellEvents(spell, status, value, source, sourceType, source, (TargetType)sourceType);
				case MagicTarget.Enemy:
					return EnemyStatusSpellEvents(spell, status, value, source, sourceType);
				case MagicTarget.Enemies:
					return EnemiesStatusSpellEvents(spell, status, value, source, sourceType);
				default:
					return Enumerable.Empty<Event>();
			}
		}

		private static IEnumerable<Event> StatusAbilityEvents(int ability, int value, int source, SourceType sourceType)
		{
			var targetType = AbilityTypes[ability].Target;
			var status = AbilityTypes[ability].EffectStatus;

			switch (targetType)
			{
				case MagicTarget.Self:
					return SelfStatusAbilityEvents(ability, status, value, source, sourceType, source, (TargetType)sourceType);
				case MagicTarget.Enemy:
					return EnemyStatusAbilityEvents(ability, status, value, source, sourceType);
				case MagicTarget.Enemies:
					return EnemiesStatusAbilityEvents(ability, status, value, source, sourceType);
				default:
					return Enumerable.Empty<Event>();
			}
		}

		private static IEnumerable<Event> ForceStatusSpellEvents(int spell, int value, int source, SourceType sourceType)
		{
			var targetType = SpellTypes[spell].Target;
			var status = SpellTypes[spell].EffectStatus;

			switch (targetType)
			{
				//case MagicTarget.Self:
				//	return SelfForceStatusSpellEvents(spell, status, value, source, sourceType, source, (TargetType)sourceType);
				//case MagicTarget.Enemies:
				//	return EnemiesForceStatusSpellEvents(spell, status, value, source, sourceType);
				case MagicTarget.Enemy:
					return EnemyForceStatusSpellEvents(spell, status, value, source, sourceType);
				default:
					return Enumerable.Empty<Event>();
			}
		}

		private static IEnumerable<Event> EnemyForceStatusSpellEvents(int spell, Status status, int value, int source, SourceType sourceType)
		{
			if (sourceType == SourceType.Enemy)
			{
				var ally = BattleLogic.RandomAlly(Random.Next(256));

				while (AllyStatuses[ally].HasFlag(Status.Stone) ||
					AllyStatuses[ally].HasFlag(Status.Dead))
					ally = BattleLogic.RandomAlly(Random.Next(256));

				yield return new Event { Type = EventType.Spell, Value = spell, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };

				AllyStatuses[ally] |= status;

				yield return new Event { Type = EventType.Status, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };
			}
			else
			{
				yield break;
			}
		}

		private static IEnumerable<Event> EnemyStatusSpellEvents(int spell, Status status, int value, int source, SourceType sourceType)
		{
			if (sourceType == SourceType.Enemy)
			{
				var ally = BattleLogic.RandomAlly(Random.Next(256));

				while (AllyStatuses[ally].HasFlag(Status.Stone) ||
					AllyStatuses[ally].HasFlag(Status.Dead))
					ally = BattleLogic.RandomAlly(Random.Next(256));

				yield return new Event { Type = EventType.Spell, Value = spell, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };

				AllyStatuses[ally] |= status;

				yield return new Event { Type = EventType.Status, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };
			}
			else
			{
				yield break;
			}
		}

		private static IEnumerable<Event> EnemyStatusAbilityEvents(int ability, Status status, int value, int source, SourceType sourceType)
		{
			if (sourceType == SourceType.Enemy)
			{
				var ally = BattleLogic.RandomAlly(Random.Next(256));

				while (AllyStatuses[ally].HasFlag(Status.Stone) ||
					AllyStatuses[ally].HasFlag(Status.Dead))
					ally = BattleLogic.RandomAlly(Random.Next(256));

				yield return new Event { Type = EventType.Ability, Value = ability, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };

				AllyStatuses[ally] |= status;

				if (status == Status.Dead)
					Party.Characters[ally].Health = 0;

				yield return new Event { Type = EventType.Status, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };
			}
			else
			{
				yield break;
			}
		}

		private static IEnumerable<Event> EnemiesStatusSpellEvents(int spell, Status status, int value, int source, SourceType sourceType)
		{
			if (sourceType == SourceType.Enemy)
			{
				for (var ally = 0; ally < Party.Characters.Length; ally++)
				{
					if (AllyStatuses[ally].HasFlag(Status.Stone) ||
						AllyStatuses[ally].HasFlag(Status.Dead))
						continue;

					yield return new Event { Type = EventType.Spell, Value = spell, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };

					AllyStatuses[ally] |= status;

					yield return new Event { Type = EventType.Status, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };
				}
			}
			else
			{
				yield break;
			}
		}

		private static IEnumerable<Event> EnemiesStatusAbilityEvents(int ability, Status status, int value, int source, SourceType sourceType)
		{
			if (sourceType == SourceType.Enemy)
			{
				for (var ally = 0; ally < Party.Characters.Length; ally++)
				{
					if (AllyStatuses[ally].HasFlag(Status.Stone) ||
						AllyStatuses[ally].HasFlag(Status.Dead))
						continue;

					yield return new Event { Type = EventType.Ability, Value = ability, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };

					AllyStatuses[ally] |= status;

					yield return new Event { Type = EventType.Status, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };
				}
			}
			else
			{
				yield break;
			}
		}

		private static IEnumerable<Event> SelfStatusSpellEvents(int spell, Status status, int value, int source, SourceType sourceType, int target, TargetType targetType)
		{
			yield break;
		}

		private static IEnumerable<Event> SelfStatusAbilityEvents(int ability, Status status, int value, int source, SourceType sourceType, int target, TargetType targetType)
		{
			yield break;
		}

		private static IEnumerable<Event> EnemyDamageSpellEvents(int spell, int value, int source, SourceType sourceType)
		{
			if (sourceType == SourceType.Enemy)
			{
				var ally = BattleLogic.RandomAlly(Random.Next(256));

				while (AllyStatuses[ally].HasFlag(Status.Stone) ||
					AllyStatuses[ally].HasFlag(Status.Dead))
					ally = BattleLogic.RandomAlly(Random.Next(256));

				yield return new Event { Type = EventType.Spell, Value = spell, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };

				var damage = Random.Next(value, value * 2);

				if (damage < Party.Characters[ally].Health)
					Party.Characters[ally].Health -= damage;
				else
				{
					Party.Characters[ally].Health = 0;
					AllyStatuses[ally] |= Status.Dead;
				}

				yield return new Event { Type = EventType.Health, Value = -damage, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };
			}
		}

		private static IEnumerable<Event> EnemiesDamageSpellEvents(int spell, int value, int source, SourceType sourceType)
		{
			if (sourceType == SourceType.Enemy)
			{
				for (var ally = 0; ally < Party.Characters.Length; ally++)
				{
					if (AllyStatuses[ally].HasFlag(Status.Stone) ||
						AllyStatuses[ally].HasFlag(Status.Dead))
						continue;

					yield return new Event { Type = EventType.Spell, Value = spell, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };

					var damage = Random.Next(value, value * 2);

					if (damage < Party.Characters[ally].Health)
						Party.Characters[ally].Health -= damage;
					else
					{
						Party.Characters[ally].Health = 0;
						AllyStatuses[ally] |= Status.Dead;
					}

					yield return new Event { Type = EventType.Health, Value = -damage, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };
				}
			}
		}

		private static IEnumerable<Event> EnemyDamageAbilityEvents(int ability, int value, int source, SourceType sourceType)
		{
			if (sourceType == SourceType.Enemy)
			{
				var ally = BattleLogic.RandomAlly(Random.Next(256));

				while (AllyStatuses[ally].HasFlag(Status.Stone) ||
					AllyStatuses[ally].HasFlag(Status.Dead))
					ally = BattleLogic.RandomAlly(Random.Next(256));

				yield return new Event { Type = EventType.Ability, Value = ability, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };

				var damage = Random.Next(value, value * 2);

				if (damage < Party.Characters[ally].Health)
					Party.Characters[ally].Health -= damage;
				else
				{
					Party.Characters[ally].Health = 0;
					AllyStatuses[ally] |= Status.Dead;
				}

				yield return new Event { Type = EventType.Health, Value = -damage, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };
			}
		}

		private static IEnumerable<Event> EnemiesDamageAbilityEvents(int ability, int value, int source, SourceType sourceType)
		{
			if (sourceType == SourceType.Enemy)
			{
				for (var ally = 0; ally < Party.Characters.Length; ally++)
				{
					if (AllyStatuses[ally].HasFlag(Status.Stone) ||
						AllyStatuses[ally].HasFlag(Status.Dead))
						continue;

					yield return new Event { Type = EventType.Ability, Value = ability, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };

					var damage = Random.Next(value, value * 2);

					if (damage < Party.Characters[ally].Health)
						Party.Characters[ally].Health -= damage;
					else
					{
						Party.Characters[ally].Health = 0;
						AllyStatuses[ally] |= Status.Dead;
					}

					yield return new Event { Type = EventType.Health, Value = -damage, Source = source, SourceType = sourceType, Target = ally, TargetType = TargetType.Ally };
				}
			}
		}

		private static IEnumerable<Event> SelfEvadeSpellEvents(int spell, int value, int source, SourceType sourceType, int target, TargetType targetType)
		{
			if (targetType == TargetType.Enemy)
				Enemies[target].Evade += value;
			else
				AllyEvades[target] += value;

			yield return new Event { Type = EventType.Spell, Value = spell, Source = source, SourceType = sourceType, Target = target, TargetType = targetType };
			yield return new Event { Type = EventType.Evade, Value = value, Source = source, SourceType = sourceType, Target = target, TargetType = targetType };
		}

		private static IEnumerable<Event> SelfFastSpellEvents(int spell, int value, int source, SourceType sourceType, int target, TargetType targetType)
		{
			yield return new Event { Type = EventType.Spell, Value = spell, Source = source, SourceType = sourceType, Target = target, TargetType = targetType };

			if (targetType == TargetType.Enemy)
			{
				if (EnemyHitMultipliers[target] < 2)
				{
					EnemyHitMultipliers[target]++;
					//yield return new Event { Type = EventType., Value = value, Source = source, SourceType = sourceType, Target = target, TargetType = targetType };
				}
			}
			else
			{
				if (AllyHitMultipliers[target] < 2)
				{
					AllyHitMultipliers[target]++;
					//yield return new Event { Type = EventType., Value = value, Source = source, SourceType = sourceType, Target = target, TargetType = targetType };
				}
			}

		}

		private static IEnumerable<Event> AllyFastSpellEvents(int spell, int value, int source, SourceType sourceType)
		{
			if (sourceType == SourceType.Enemy)
			{
				var target = Random.Next(10);

				while (target >= Enemies.Length ||
					EnemyStatuses[target].HasFlag(Status.Dead))
					target = Random.Next(10);

				yield return new Event { Type = EventType.Spell, Value = spell, Source = source, SourceType = sourceType, Target = target, TargetType = TargetType.Enemy };

				if (EnemyHitMultipliers[target] < 2)
				{
					EnemyHitMultipliers[target]++;
					//yield return new Event { Type = EventType., Value = value, Source = source, SourceType = sourceType, Target = target, TargetType = targetType };
				}
			}
			else
			{
				var target = Random.Next(4);

				yield return new Event { Type = EventType.Spell, Value = spell, Source = source, SourceType = sourceType, Target = target, TargetType = TargetType.Enemy };

				// TODO: Check flags
				if (AllyHitMultipliers[target] < 2)
				{
					AllyHitMultipliers[target]++;
					//yield return new Event { Type = EventType., Value = value, Source = source, SourceType = sourceType, Target = target, TargetType = targetType };
				}
			}

		}

		public static void Update()
		{
			if (Enumerable.Range(0, Party.Characters.Length)
				.All(x => AllyActions[x] != -1 ||
					AllyStatuses[x].HasFlag(Status.Sleep) ||
					AllyStatuses[x].HasFlag(Status.Stone) ||
					AllyStatuses[x].HasFlag(Status.Dead) ||
					AllyStatuses[x].HasFlag(Status.Stun)))
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
			internal int Evade;
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
			Ally,
			Enemies,
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
			Status,
			Cure,
			Resist,
			Weak,
			Escape,
			Trapped,
			Evade
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
			public string Name;
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
			ForceStatus
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

		public struct FormationType
		{
			public int Type;
			public int Minimum;
			public int Maximum;
			public int AlternateMinimum;
			public int AlternateMaximum;
		}
	}
}
