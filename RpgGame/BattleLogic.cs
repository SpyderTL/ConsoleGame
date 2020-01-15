using System;
using System.Collections.Generic;
using System.Text;

namespace RpgGame
{
	public static class BattleLogic
	{
		public static void Enable()
		{
			Battle.TurnStarting += Battle_TurnStarting;
		}

		private static void Battle_TurnStarting()
		{
			var random = new Random();

			for (var enemy = 0; enemy < Battle.Enemies.Length; enemy++)
				Battle.EnemyActions[enemy] = random.Next(0, Battle.EnemyOptions[enemy].Length);
		}

		public static void Disable()
		{
			Battle.TurnStarting -= Battle_TurnStarting;
		}
	}
}
