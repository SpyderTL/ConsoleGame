using System;
using System.Linq;
using System.Threading;

namespace ConsoleGame
{
	internal class BattleScreen
	{
		private static AutoResetEvent Close = new AutoResetEvent(false);

		internal static void Show()
		{
			Banner();

			Draw();
			Screen.Update();

			Close.WaitOne();
		}

		internal static void Hide()
		{
			Close.Set();
		}

		internal static void Draw()
		{
			for (var ally = 0; ally < Battle.Allies.Length; ally++)
			{
				Screen.DrawString(Battle.Allies[ally].Name, 0, ally);
			}

			for (var enemy = 0; enemy < Battle.Enemies.Length; enemy++)
			{
				Screen.DrawString(Battle.Enemies[enemy].Name, 20, enemy);
			}

			if (BattleMenu.Character != -1)
			{
				Screen.FillRectangle(' ', BattleMenu.Character + 1, 11, BattleMenu.Character + BattleMenu.Items.Length, 20);
				Screen.DrawRectangle('<', BattleMenu.Character, 10, BattleMenu.Character + BattleMenu.Items.Length + 1, 21);

				for (var item = 0; item < BattleMenu.Items.Length; item++)
					Screen.DrawString((item + 1).ToString() + ") " + BattleMenu.Items[item].Text, 12, BattleMenu.Character + item);
			}
		}

		internal static void Banner()
		{
			Screen.FillRectangle('#', 10, 0, 14, 39);

			Screen.DrawString("Party Attacked", 12, 12);
			Screen.Update();

			var wait = Environment.TickCount + 200;

			while (Environment.TickCount < wait)
				Thread.Sleep(10);

			for (var x = 0; x < 40; x++)
			{
				Screen.DrawVerticalLine(' ', x, 15, 24);
				Screen.DrawVerticalLine(' ', 39 - x, 0, 9);
				Screen.Update();

				wait = Environment.TickCount + 20;

				while (Environment.TickCount < wait)
					Thread.Sleep(10);
			}

			wait = Environment.TickCount + 200;

			while (Environment.TickCount < wait)
				Thread.Sleep(10);

			Screen.Clear();
			Screen.Update();
		}
	}
}