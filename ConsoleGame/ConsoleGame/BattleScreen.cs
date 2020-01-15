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
			Screen.Clear();

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
				Screen.FillRectangle(' ', BattleMenu.Character + 1, 7, BattleMenu.Character + BattleMenu.Items.Length + 2, 17);
				Screen.DrawRectangle('<', BattleMenu.Character, 6, BattleMenu.Character + BattleMenu.Items.Length + 3, 18);

				for (var item = 0; item < BattleMenu.Items.Length; item++)
					Screen.DrawString((item + 1).ToString() + ") " + BattleMenu.Items[item].Text, 8, BattleMenu.Character + 2 + item);
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

		internal static void DrawEvents()
		{
			foreach (var e in Battle.Events)
			{
				switch (e.Type)
				{
					case Battle.EventType.Attack:
						Screen.FillRectangle(' ', 10, 7, 14, 15);
						Screen.DrawRectangle('#', 9, 6, 15, 16);

						Screen.DrawString(e.SourceType == Battle.SourceType.Ally ? Battle.Allies[e.Source].Name : Battle.Enemies[e.Source].Name, 8, 12);
						Screen.Update();

						Thread.Sleep(100);

						Screen.FillRectangle(' ', 10, 18, 14, 26);
						Screen.DrawRectangle('#', 9, 17, 15, 27);

						Screen.DrawString(e.TargetType == Battle.TargetType.Ally ? Battle.Allies[e.Target].Name : Battle.Enemies[e.Target].Name, 19, 12);
						Screen.Update();

						Thread.Sleep(100);
						break;

					case Battle.EventType.Special:
						break;

					case Battle.EventType.Magic:
						break;

					case Battle.EventType.Item:
						break;

					case Battle.EventType.Run:
						break;

					case Battle.EventType.Miss:
						Screen.FillRectangle(' ', 17, 7, 21, 15);
						Screen.DrawRectangle('#', 16, 6, 22, 16);

						Screen.DrawString("MISS", 8, 19);
						Screen.Update();

						Thread.Sleep(1500);

						Draw();
						Screen.Update();

						Thread.Sleep(500);
						break;

					case Battle.EventType.Health:
						Screen.FillRectangle(' ', 17, 7, 21, 15);
						Screen.DrawRectangle('#', 16, 6, 22, 16);

						Screen.DrawString(e.Value.ToString() + " HP", 8, 19);
						Screen.Update();

						Thread.Sleep(1500);

						Draw();
						Screen.Update();

						Thread.Sleep(500);
						break;

					case Battle.EventType.Power:
						break;

					case Battle.EventType.Inflict:
						break;

					case Battle.EventType.Cure:
						break;

					case Battle.EventType.Resist:
						break;

					case Battle.EventType.Weak:
						break;

					case Battle.EventType.Escape:
						break;

					case Battle.EventType.Trapped:
						break;
				}
			}
		}
	}
}