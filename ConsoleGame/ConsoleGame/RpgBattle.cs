using System;
using System.Linq;

namespace ConsoleGame
{
	internal class RpgBattle
	{
		internal static void Load()
		{
			Battle.AbilityNames = RpgGame.Battle.AbilityTypes.Select(x => "Ability").ToArray();
			Battle.SpellNames = RpgGame.Battle.SpellTypes.Select(x => "Spell").ToArray();
			Battle.ItemNames = RpgGame.Battle.PotionTypes.Select(x => "Item").ToArray();

			Battle.Allies = new Battle.Character[Party.Characters.Length];

			for (var ally = 0; ally < Battle.Allies.Length; ally++)
			{
				Battle.Allies[ally] = new Battle.Character { Name = Party.Characters[ally].Name };
			}

			Battle.Enemies = new Battle.Character[RpgGame.Battle.Enemies.Length];

			for (var enemy = 0; enemy < Battle.Enemies.Length; enemy++)
			{
				Battle.Enemies[enemy] = new Battle.Character { Name = RpgGame.Battle.EnemyTypes[RpgGame.Battle.Enemies[enemy].Type].Name };
			}

			Battle.Actions = Enumerable.Repeat(-1, Battle.Allies.Length).ToArray();
		}

		internal static void UpdateOptions()
		{
			Battle.Options = RpgGame.Battle.AllyOptions.Select(x => x.Select(y => new Battle.Activity
			{
				Type = (Battle.ActivityType)y.Type,
				Value = y.Value,
				TargetType = (Battle.TargetType)y.TargetType,
				Target = y.Target,
			}).ToArray())
			.ToArray();
		}

		internal static void UpdateActions()
		{
			RpgGame.Battle.AllyActions = Battle.Actions;
		}

		internal static void UpdateEvents()
		{
			Battle.Events = RpgGame.Battle.Events.Select(x => new Battle.Event
			{
				Type = (Battle.EventType)x.Type,
				SourceType = (Battle.SourceType)x.SourceType,
				Source = x.Source,
				TargetType = (Battle.TargetType)x.TargetType,
				Target = x.Target,
				Value = x.Value
			}).ToArray();
		}

		internal static void Enable()
		{
			RpgGame.Battle.BattleStarting += Battle_BattleStarting;
			RpgGame.Battle.BattleComplete += Battle_BattleComplete;
			RpgGame.Battle.TurnStarting += Battle_TurnStarting;
			RpgGame.Battle.TurnComplete += Battle_TurnComplete;
		}

		private static void Battle_BattleStarting()
		{
			BattleScreen.DrawBanner();
			BattleScreen.DrawSlow();
		}

		private static void Battle_BattleComplete()
		{
			BattleScreen.Hide();
		}

		private static void Battle_TurnStarting()
		{
			UpdateOptions();

			BattleMenu.Character = 0;

			BattleMenu.ActivityType = -1;
			BattleMenu.Activity = -1;

			BattleMenu.Update();

			BattleScreen.Draw();
			Screen.Update();
		}

		private static void Battle_TurnComplete()
		{
			Battle.Mode = Battle.BattleMode.TurnComplete;

			BattleMenu.Character = -1;

			BattleMenu.ActivityType = -1;
			BattleMenu.Activity = -1;

			BattleMenu.Update();

			UpdateEvents();
			BattleScreen.DrawEvents();
		}

		internal static void Disable()
		{
			RpgGame.Battle.BattleStarting -= Battle_BattleStarting;
			RpgGame.Battle.BattleComplete -= Battle_BattleComplete;
			RpgGame.Battle.TurnStarting -= Battle_TurnStarting;
			RpgGame.Battle.TurnComplete -= Battle_TurnComplete;
		}
	}
}