using System;
using System.Linq;

namespace ConsoleGame
{
	internal class RpgBattle
	{
		internal static void Load()
		{
			Battle.Allies = new Battle.Character[Party.Characters.Length];

			for (var ally = 0; ally < Battle.Allies.Length; ally++)
			{
				Battle.Allies[ally] = new Battle.Character { Name = Party.Characters[ally].Name };
			}

			Battle.Enemies = new Battle.Character[RpgGame.Battle.Enemies.Length];

			for (var enemy = 0; enemy < Battle.Enemies.Length; enemy++)
			{
				Battle.Enemies[enemy] = new Battle.Character { Name = RpgGame.Battle.Enemies[enemy].Type.ToString() };
			}
		}

		internal static void UpdateActions()
		{
			RpgGame.Battle.AllyActions = Battle.AllyActions;
		}

		internal static void UpdateEvents()
		{
			Battle.TurnEvents = RpgGame.Battle.Events.Select(x => new Battle.Event
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
			RpgGame.Battle.BattleStarted += Battle_BattleStarted;
			RpgGame.Battle.BattleComplete += Battle_BattleComplete;
			RpgGame.Battle.TurnStarted += Battle_TurnStarted;
			RpgGame.Battle.TurnComplete += Battle_TurnComplete;
		}

		private static void Battle_BattleStarted()
		{
			Battle.Mode = Battle.BattleMode.BattleStarting;
		}

		private static void Battle_BattleComplete()
		{
			Battle.Mode = Battle.BattleMode.BattleComplete;
		}

		private static void Battle_TurnStarted()
		{
			Battle.Mode = Battle.BattleMode.TurnStarting;
		}

		private static void Battle_TurnComplete()
		{
			Battle.Mode = Battle.BattleMode.TurnComplete;


		}

		internal static void Disable()
		{
			RpgGame.Battle.BattleStarted -= Battle_BattleStarted;
			RpgGame.Battle.BattleComplete -= Battle_BattleComplete;
			RpgGame.Battle.TurnStarted -= Battle_TurnStarted;
			RpgGame.Battle.TurnComplete -= Battle_TurnComplete;
		}
	}
}