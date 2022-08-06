using System;
using System.Linq;
using System.Timers;
using Rpg;

namespace ConsoleGame
{
	internal class RpgBattle
	{
		internal static void ReadData()
		{
			Battle.AbilityNames = RpgGame.Battle.AbilityTypes.Select(x => x.Name).ToArray();
			Battle.SpellNames = RpgGame.Battle.SpellTypes.Select(x => x.Name).ToArray();
			Battle.ItemNames = RpgGame.Battle.PotionTypes.Select(x => x.Name).ToArray();

			Battle.Allies = new Battle.Character[Party.Characters.Length];

			for (var ally = 0; ally < Battle.Allies.Length; ally++)
			{
				Battle.Allies[ally] = new Battle.Character { Name = RpgGame.Party.Characters[ally].Name };
			}

			Battle.Enemies = new Battle.Character[RpgGame.Battle.Enemies.Length];

			for (var enemy = 0; enemy < Battle.Enemies.Length; enemy++)
			{
				Battle.Enemies[enemy] = new Battle.Character { Name = RpgGame.Battle.EnemyTypes[RpgGame.Battle.Enemies[enemy].Type].Name };
			}

			Battle.Actions = Enumerable.Repeat(-1, Battle.Allies.Length).ToArray();
		}

		internal static void ReadCharacters()
		{
			for (var ally = 0; ally < Battle.Allies.Length; ally++)
			{
				Battle.Allies[ally].Health = RpgGame.Party.Characters[ally].Health;
				Battle.Allies[ally].MaxHealth = RpgGame.Party.Characters[ally].MaxHealth;
				Battle.Allies[ally].Power = RpgGame.Party.Characters[ally].Power;
				Battle.Allies[ally].MaxPower = RpgGame.Party.Characters[ally].MaxPower;
			}

			for (var enemy = 0; enemy < Battle.Enemies.Length; enemy++)
			{
				Battle.Enemies[enemy].Health = RpgGame.Battle.Enemies[enemy].Health;
				Battle.Enemies[enemy].MaxHealth = RpgGame.Battle.Enemies[enemy].MaxHealth;
				Battle.Enemies[enemy].Power = RpgGame.Battle.Enemies[enemy].Power;
				Battle.Enemies[enemy].MaxPower = RpgGame.Battle.Enemies[enemy].MaxPower;
			}
		}

		internal static void ReadOptions()
		{
			Battle.Options = RpgGame.Battle.AllyOptions
				.Select(x => x.Select(y => new Battle.Activity
				{
					Type = (Battle.ActivityType)y.Type,
					Value = y.Value,
					TargetType = (Battle.TargetType)y.TargetType,
					Target = y.Target,
				}).ToArray())
			.ToArray();
		}

		internal static void ReadEvents()
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
			if (RpgGame.Battle.Result == RpgGame.Battle.BattleResults.Defeat)
				Game.Mode = Game.GameMode.Intro;
			else
			{
				Game.Mode = Game.GameMode.World;

				RpgParty.Refresh();
			}

			BattleScreen.Hide();
		}

		private static void Battle_TurnStarting()
		{
			ReadOptions();

			for (int i = 0; i < Party.Characters.Length; i++)
				Battle.Actions[i] = -1;

			BattleMenu.Character = Enumerable.Range(0, Party.Characters.Length)
				.Where(x => !RpgGame.Battle.AllyStatuses[x].HasFlag(RpgGame.Battle.Status.Stone) &&
					!RpgGame.Battle.AllyStatuses[x].HasFlag(RpgGame.Battle.Status.Dead) &&
					!RpgGame.Battle.AllyStatuses[x].HasFlag(RpgGame.Battle.Status.Stun) &&
					!RpgGame.Battle.AllyStatuses[x].HasFlag(RpgGame.Battle.Status.Sleep))
				.DefaultIfEmpty(-1)
				.First();

			BattleMenu.ActivityType = -1;
			BattleMenu.Activity = -1;

			BattleMenu.Update();

			BattleScreen.Draw();
			Screen.Update();

			if(BattleMenu.Character == -1)
				WriteActions();
		}

		private static void Battle_TurnComplete()
		{
			Battle.Mode = Battle.BattleMode.TurnComplete;

			BattleMenu.Character = -1;

			BattleMenu.ActivityType = -1;
			BattleMenu.Activity = -1;

			BattleMenu.Update();
			BattleScreen.Draw();

			ReadEvents();
			BattleScreen.DrawEvents();
		}

		internal static void WriteActions()
		{
			for (int i = 0; i < Party.Characters.Length; i++)
				RpgGame.Battle.AllyActions[i] = Battle.Actions[i];

			RpgGame.Battle.Update();
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