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
			Close.WaitOne();
		}

		internal static void Hide()
		{
			Close.Set();
		}

		internal static void DrawSlow()
		{
			Screen.Clear();
			Screen.Update();
			Thread.Sleep(500);

			Screen.DrawRectangle('>', 0, 0, 7, 8);
			Screen.Update();
			Thread.Sleep(500);

			Screen.DrawRectangle('<', 0, 24, 12, 39);
			Screen.Update();
			Thread.Sleep(500);

			for (var ally = 0; ally < Battle.Allies.Length; ally++)
			{
				Screen.DrawString(Battle.Allies[ally].Name, 2, ally + 2);
				Screen.Update();
				Thread.Sleep(100);
			}

			Thread.Sleep(500);

			for (var enemy = 0; enemy < Battle.Enemies.Length; enemy++)
			{
				Screen.DrawString(Battle.Enemies[enemy].Name, 26, enemy + 2);
				Screen.Update();
				Thread.Sleep(100);
			}

			Thread.Sleep(500);
		}

		internal static void Draw()
		{
			Screen.Clear();

			Screen.DrawRectangle('>', 0, 0, 7, 8);
			Screen.DrawRectangle('<', 0, 24, 12, 39);

			for (var ally = 0; ally < Battle.Allies.Length; ally++)
			{
				Screen.DrawString(Battle.Allies[ally].Name, 2, ally + 2);
			}

			for (var enemy = 0; enemy < Battle.Enemies.Length; enemy++)
			{
				Screen.DrawString(Battle.Enemies[enemy].Name, 26, enemy + 2);
			}

			if (BattleMenu.Character != -1)
			{
				Screen.FillRectangle(' ', BattleMenu.Character + 3, 10, BattleMenu.Character + BattleMenu.Items.Length + 4, 22);
				Screen.DrawRectangle('>', BattleMenu.Character + 2, 9, BattleMenu.Character + BattleMenu.Items.Length + 5, 23);

				for (var item = 0; item < BattleMenu.Items.Length; item++)
					Screen.DrawString((item + 1).ToString() + ") " + BattleMenu.Items[item].Text, 11, BattleMenu.Character + 4 + item);
			}
		}

		internal static void DrawBanner()
		{
			Screen.FillRectangle('-', 10, 0, 14, 39);

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
					case Battle.EventType.Hit:
						Screen.FillRectangle(' ', 10, 7, 14, 16);
						Screen.DrawRectangle('#', 9, 6, 15, 17);

						Screen.DrawString(e.SourceType == Battle.SourceType.Ally ? Battle.Allies[e.Source].Name : Battle.Enemies[e.Source].Name, 8, 12);
						Screen.Update();

						Thread.Sleep(500);

						Screen.FillRectangle(' ', 10, 20, 14, 29);
						Screen.DrawRectangle('#', 9, 19, 15, 30);

						Screen.DrawString(e.TargetType == Battle.TargetType.Ally ? Battle.Allies[e.Target].Name : Battle.Enemies[e.Target].Name, 21, 12);
						Screen.Update();

						Thread.Sleep(1000);

						Screen.FillRectangle(' ', 18, 7, 21, 16);
						Screen.DrawRectangle('#', 17, 6, 22, 17);

						Screen.DrawString(e.Value.ToString() + " Hit", 8, 19);
						Screen.Update();

						Thread.Sleep(500);
						break;

					case Battle.EventType.Ability:
						Screen.FillRectangle(' ', 10, 7, 14, 16);
						Screen.DrawRectangle('#', 9, 6, 15, 17);

						Screen.DrawString(e.SourceType == Battle.SourceType.Ally ? Battle.Allies[e.Source].Name : Battle.Enemies[e.Source].Name, 8, 12);
						Screen.Update();

						Thread.Sleep(500);

						Screen.FillRectangle(' ', 10, 20, 14, 29);
						Screen.DrawRectangle('#', 9, 19, 15, 30);

						Screen.DrawString(e.TargetType == Battle.TargetType.Ally ? Battle.Allies[e.Target].Name : Battle.Enemies[e.Target].Name, 21, 12);
						Screen.Update();

						Thread.Sleep(1000);

						Screen.FillRectangle(' ', 18, 7, 21, 16);
						Screen.DrawRectangle('#', 17, 6, 22, 17);

						Screen.DrawString(Battle.AbilityNames[e.Value], 8, 19);
						Screen.Update();

						Thread.Sleep(500);
						break;

					case Battle.EventType.Spell:
						Screen.FillRectangle(' ', 10, 7, 14, 16);
						Screen.DrawRectangle('#', 9, 6, 15, 17);

						Screen.DrawString(e.SourceType == Battle.SourceType.Ally ? Battle.Allies[e.Source].Name : Battle.Enemies[e.Source].Name, 8, 12);
						Screen.Update();

						Thread.Sleep(500);

						Screen.FillRectangle(' ', 10, 20, 14, 29);
						Screen.DrawRectangle('#', 9, 19, 15, 30);

						Screen.DrawString(e.TargetType == Battle.TargetType.Ally ? Battle.Allies[e.Target].Name : Battle.Enemies[e.Target].Name, 21, 12);
						Screen.Update();

						Thread.Sleep(1000);

						Screen.FillRectangle(' ', 18, 7, 21, 16);
						Screen.DrawRectangle('#', 17, 6, 22, 17);

						Screen.DrawString(Battle.SpellNames[e.Value], 8, 19);
						Screen.Update();

						Thread.Sleep(500);
						break;

					case Battle.EventType.Item:
						Screen.FillRectangle(' ', 10, 7, 14, 16);
						Screen.DrawRectangle('#', 9, 6, 15, 17);

						Screen.DrawString(e.SourceType == Battle.SourceType.Ally ? Battle.Allies[e.Source].Name : Battle.Enemies[e.Source].Name, 8, 12);
						Screen.Update();

						Thread.Sleep(500);

						Screen.FillRectangle(' ', 10, 20, 14, 29);
						Screen.DrawRectangle('#', 9, 19, 15, 30);

						Screen.DrawString(e.TargetType == Battle.TargetType.Ally ? Battle.Allies[e.Target].Name : Battle.Enemies[e.Target].Name, 21, 12);
						Screen.Update();

						Thread.Sleep(1000);

						Screen.FillRectangle(' ', 18, 7, 21, 16);
						Screen.DrawRectangle('#', 17, 6, 22, 17);

						Screen.DrawString(Battle.ItemNames[e.Value], 8, 19);
						Screen.Update();

						Thread.Sleep(500);
						break;

					case Battle.EventType.Run:
						Screen.FillRectangle(' ', 10, 7, 14, 16);
						Screen.DrawRectangle('#', 9, 6, 15, 17);

						Screen.DrawString(e.SourceType == Battle.SourceType.Ally ? Battle.Allies[e.Source].Name : Battle.Enemies[e.Source].Name, 8, 12);
						Screen.Update();

						Thread.Sleep(500);

						Screen.FillRectangle(' ', 10, 20, 14, 29);
						Screen.DrawRectangle('#', 9, 19, 15, 30);

						Screen.DrawString("Run Away", 19, 12);
						Screen.Update();

						Thread.Sleep(1000);
						break;

					case Battle.EventType.Miss:
						Screen.FillRectangle(' ', 10, 7, 14, 16);
						Screen.DrawRectangle('#', 9, 6, 15, 17);

						Screen.DrawString(e.SourceType == Battle.SourceType.Ally ? Battle.Allies[e.Source].Name : Battle.Enemies[e.Source].Name, 8, 12);
						Screen.Update();

						Thread.Sleep(500);

						Screen.FillRectangle(' ', 10, 20, 14, 29);
						Screen.DrawRectangle('#', 9, 19, 15, 30);

						Screen.DrawString(e.TargetType == Battle.TargetType.Ally ? Battle.Allies[e.Target].Name : Battle.Enemies[e.Target].Name, 21, 12);
						Screen.Update();

						Thread.Sleep(1000);

						Screen.FillRectangle(' ', 18, 7, 21, 16);
						Screen.DrawRectangle('#', 17, 6, 22, 17);

						Screen.DrawString("MISS", 8, 19);
						Screen.Update();

						Thread.Sleep(1500);

						Draw();
						Screen.Update();

						Thread.Sleep(500);
						break;

					case Battle.EventType.Health:
						Screen.FillRectangle(' ', 18, 20, 21, 29);
						Screen.DrawRectangle('#', 17, 19, 22, 30);

						Screen.DrawString(Math.Abs(e.Value).ToString() + " HP", 21, 19);
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
						Screen.FillRectangle(' ', 18, 7, 21, 15);
						Screen.DrawRectangle('#', 17, 6, 22, 16);

						Screen.DrawString("Escaped!", 8, 19);
						Screen.Update();

						Thread.Sleep(1500);
						break;

					case Battle.EventType.Trapped:
						Screen.FillRectangle(' ', 18, 7, 21, 15);
						Screen.DrawRectangle('#', 17, 6, 22, 16);

						Screen.DrawString("Trapped!", 8, 19);
						Screen.Update();

						Thread.Sleep(1500);
						break;
				}
			}
		}
	}
}