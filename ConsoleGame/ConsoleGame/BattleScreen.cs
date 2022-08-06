using System;
using System.Linq;
using System.Threading;
using Rpg;

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

			Screen.DrawRectangle('#', 0, 0, 18, 14);
			Screen.Update();
			Thread.Sleep(500);

			Screen.DrawRectangle('#', 0, 24, 12, 39);
			Screen.Update();
			Thread.Sleep(500);

			for (var ally = 0; ally < Battle.Allies.Length; ally++)
			{
				Screen.DrawString(Battle.Allies[ally].Name, 2, (ally * 4) + 2);

				Screen.DrawString(Battle.Allies[ally].Health.ToString(), 4, (ally * 4) + 3);
				Screen.DrawString("/", 7, (ally * 4) + 3);
				Screen.DrawString(Battle.Allies[ally].MaxHealth.ToString(), 8, (ally * 4) + 3);
				Screen.DrawString("HP", 11, (ally * 4) + 3);

				Screen.DrawString(Battle.Allies[ally].Power.ToString(), 4, (ally * 4) + 4);
				Screen.DrawString("/", 7, (ally * 4) + 4);
				Screen.DrawString(Battle.Allies[ally].MaxPower.ToString(), 8, (ally * 4) + 4);
				Screen.DrawString("MP", 11, (ally * 4) + 4);

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

			Screen.DrawRectangle('#', 0, 0, 18, 14);
			Screen.DrawRectangle('#', 0, 24, 12, 39);

			for (var ally = 0; ally < Battle.Allies.Length; ally++)
			{
				Screen.DrawString(Battle.Allies[ally].Name, 2, (ally * 4) + 2);

				Screen.DrawString(Battle.Allies[ally].Health.ToString(), 4, (ally * 4) + 3);
				Screen.DrawString("/", 7, (ally * 4) + 3);
				Screen.DrawString(Battle.Allies[ally].MaxHealth.ToString(), 8, (ally * 4) + 3);
				Screen.DrawString("HP", 11, (ally * 4) + 3);

				Screen.DrawString(Battle.Allies[ally].Power.ToString(), 4, (ally * 4) + 4);
				Screen.DrawString("/", 7, (ally * 4) + 4);
				Screen.DrawString(Battle.Allies[ally].MaxPower.ToString(), 8, (ally * 4) + 4);
				Screen.DrawString("MP", 11, (ally * 4) + 4);
			}

			for (var enemy = 0; enemy < Battle.Enemies.Length; enemy++)
			{
				if(Battle.Enemies[enemy].Health != 0)
					Screen.DrawString(Battle.Enemies[enemy].Name, 26, enemy + 2);
			}

			if (BattleMenu.Character != -1)
			{
				Screen.FillRectangle(' ', (BattleMenu.Character * 4) + 1, 14, (BattleMenu.Character * 4) + BattleMenu.Items.Length + 2, 26);
				Screen.DrawRectangle('#', (BattleMenu.Character * 4) + 0, 13, (BattleMenu.Character * 4) + BattleMenu.Items.Length + 3, 27);

				for (var item = 0; item < BattleMenu.Items.Length; item++)
					Screen.DrawString((item + 1).ToString() + ") " + BattleMenu.Items[item].Text, 15, (BattleMenu.Character * 4) + 2 + item);
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

						if (e.TargetType == Battle.TargetType.Ally)
						{
							Battle.Allies[e.Target].Health += e.Value;

							if (Battle.Allies[e.Target].Health < 0)
								Battle.Allies[e.Target].Health = 0;
							else if (Battle.Allies[e.Target].Health > Battle.Allies[e.Target].MaxHealth)
								Battle.Allies[e.Target].Health = Battle.Allies[e.Target].MaxHealth;
						}
						else
						{
							Battle.Enemies[e.Target].Health += e.Value;

							if (Battle.Enemies[e.Target].Health < 0)
								Battle.Enemies[e.Target].Health = 0;
							else if (Battle.Enemies[e.Target].Health > Battle.Enemies[e.Target].MaxHealth)
								Battle.Enemies[e.Target].Health = Battle.Enemies[e.Target].MaxHealth;
						}

						Draw();
						Screen.Update();

						Thread.Sleep(500);
						break;

					case Battle.EventType.Evade:
						Screen.FillRectangle(' ', 18, 20, 21, 29);
						Screen.DrawRectangle('#', 17, 19, 22, 30);

						Screen.DrawString("Evade Up", 21, 19);
						Screen.Update();

						Thread.Sleep(1500);

						if (e.TargetType == Battle.TargetType.Ally)
						{
							Battle.Allies[e.Target].Health += e.Value;

							if (Battle.Allies[e.Target].Health < 0)
								Battle.Allies[e.Target].Health = 0;
							else if (Battle.Allies[e.Target].Health > Battle.Allies[e.Target].MaxHealth)
								Battle.Allies[e.Target].Health = Battle.Allies[e.Target].MaxHealth;
						}
						else
						{
							Battle.Enemies[e.Target].Health += e.Value;

							if (Battle.Enemies[e.Target].Health < 0)
								Battle.Enemies[e.Target].Health = 0;
							else if (Battle.Enemies[e.Target].Health > Battle.Enemies[e.Target].MaxHealth)
								Battle.Enemies[e.Target].Health = Battle.Enemies[e.Target].MaxHealth;
						}

						Draw();
						Screen.Update();

						Thread.Sleep(500);
						break;

					case Battle.EventType.Power:
						break;

					case Battle.EventType.Status:
						//Screen.FillRectangle(' ', 18, 7, 21, 15);
						//Screen.DrawRectangle('#', 17, 6, 22, 16);

						//Screen.DrawString("Status!", 8, 19);
						//Screen.Update();

						//Thread.Sleep(1500);

						Draw();
						Screen.Update();
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