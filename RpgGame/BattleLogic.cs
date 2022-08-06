using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	internal static class BattleLogic
	{
		internal static Battle.ActivityType EnemyActivityType(int logicType, Battle.Status status, int random, int random2)
		{
			if (status.HasFlag(Battle.Status.Stone) ||
				status.HasFlag(Battle.Status.Dead) ||
				status.HasFlag(Battle.Status.Sleep))
				return Battle.ActivityType.None;

			if (logicType == 255)
				return Battle.ActivityType.Attack;
			else
			{
				var spellChance = Battle.LogicTypes[logicType].Spell;
				var abilityChance = Battle.LogicTypes[logicType].Ability;

				if (random < spellChance)
					return Battle.ActivityType.Spell;
				else if (random2 < abilityChance)
					return Battle.ActivityType.Ability;
				else
					return Battle.ActivityType.Attack;
			}
		}

		internal static int RandomAlly(int random)
		{
			var ally = 0;

			if (random < 0x20)
				ally++;

			if (random < 0x40)
				ally++;

			if (random < 0x80)
				ally++;

			return ally;
		}

		//internal static Battle.Activity EnemyAction(int enemy)
		//{
		//	if (Enemies[enemy].Statuses.HasFlag(Status.Stun) ||
		//		Enemies[enemy].Statuses.HasFlag(Status.Stone) ||
		//		Enemies[enemy].Statuses.HasFlag(Status.Dead))
		//	{
		//		return new Activity
		//		{
		//			Type = ActivityType.None
		//		};
		//	}

		//	while (true)
		//	{
		//		var target = 0;
		//		var value = Random.Next(256);

		//		if (value < 0x20)
		//			target++;

		//		if (value < 0x40)
		//			target++;

		//		if (value < 0x80)
		//			target++;

		//		if (!AllyStatuses[target].HasFlag(Status.Stone) &&
		//			!AllyStatuses[target].HasFlag(Status.Dead))
		//			return new Activity
		//			{
		//				Type = ActivityType.Attack,
		//				TargetType = TargetType.Ally,
		//				Target = target
		//			};
		//	}
		//}

		//internal static IEnumerable<Battle.Event> Magic(MagicType type, Activity action, int source, SourceType sourceType, int target, TargetType targetType, int accuracy, int hits, int damage)
		//{
		//	switch (type.Effect)
		//	{
		//		case MagicEffect.Damage:
		//			damage = action.Value;
		//			damage += Random.Next(action.Value) + 1;

		//			switch (targetType)
		//			{
		//				case TargetType.Ally:
		//					Party.Characters[target].Health -= Math.Min(damage, Party.Characters[target].Health);

		//					if (Party.Characters[target].Health == 0)
		//						AllyStatuses[target] |= Status.Dead;

		//					yield return new Event { Type = EventType.Health, Source = source, SourceType = sourceType, Target = target, TargetType = targetType, Value = -damage };
		//					break;

		//				case TargetType.Enemy:
		//					Enemies[target].Health -= Math.Min(damage, Enemies[target].Health);

		//					if (Enemies[target].Health == 0)
		//						EnemyStatuses[target] |= Status.Dead;

		//					yield return new Event { Type = EventType.Health, Source = source, SourceType = sourceType, Target = target, TargetType = targetType, Value = -damage };
		//					break;

		//				case TargetType.Allies:
		//					for (var character = 0; character < Party.Characters.Length; character++)
		//					{
		//						Party.Characters[character].Health -= Math.Min(damage, Party.Characters[character].Health);

		//						if (Party.Characters[character].Health == 0)
		//							AllyStatuses[character] |= Status.Dead;

		//						yield return new Event { Type = EventType.Health, Source = source, SourceType = sourceType, Target = character, TargetType = targetType, Value = -damage };
		//					}
		//					break;

		//				case TargetType.Enemies:
		//					for (var enemy = 0; enemy < Enemies.Length; enemy++)
		//					{
		//						Enemies[enemy].Health -= Math.Min(damage, Enemies[enemy].Health);

		//						if (Enemies[enemy].Health == 0)
		//							EnemyStatuses[enemy] |= Status.Dead;

		//						yield return new Event { Type = EventType.Health, Source = source, SourceType = sourceType, Target = enemy, TargetType = targetType, Value = -damage };
		//					}
		//					break;
		//			}
		//			break;

		//		default:
		//			System.Diagnostics.Debugger.Break();
		//			yield break;
		//	}
		//}
	}
}
